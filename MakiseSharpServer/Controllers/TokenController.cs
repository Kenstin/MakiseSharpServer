using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MakiseSharpServer.Models.Discord;
using MakiseSharpServer.Models.Settings;
using MakiseSharpServer.Services;
using MakiseSharpServer.Services.APIs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MakiseSharpServer.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IDiscordApi discordClient;
        private readonly AppSettings appSettings;

        public TokenController(AppSettings appSettings, IDiscordApi discordClient)
        {
            this.appSettings = appSettings;
            this.discordClient = discordClient;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenAsync(string code)
        {
            DiscordToken token;
            try
            {
                token = await discordClient.GetAccessTokenAsync(
                    new DiscordAccessTokenRequestDto(
                        appSettings.Discord.ClientId, appSettings.Discord.ClientSecret, code, appSettings.Discord.RedirecUri));
            }
            catch (Refit.ApiException e)
            {
                //ToDo: log it
                throw;
            }

            if (token.AccessToken == null)
            {
                return BadRequest("Wrong access code.");
            }

            var user = await discordClient.GetBasicUserInfoAsync($"Bearer {token.AccessToken}");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Token.SigningKey));

            var claims = new List<Claim>
            {
                new Claim(appSettings.Token.Claims.DiscordUserId, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(appSettings.Token.Claims.DiscordUserDiscriminator, user.Discriminator),
                new Claim(appSettings.Token.Claims.DiscordUserAvatar, user.Avatar),
            };

            var jwt = new JwtSecurityToken(
                issuer: appSettings.Token.Issuer,
                audience: appSettings.Token.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(appSettings.Token.TokenLifetime),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            //ToDo: save access&refresh token to db

            return new ObjectResult(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
    }
}