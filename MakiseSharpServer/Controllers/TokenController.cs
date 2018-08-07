using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;
using MakiseSharpServer.Models.Discord;
using MakiseSharpServer.Models.Settings;
using MakiseSharpServer.Services;
using MakiseSharpServer.Services.APIs;
using Microsoft.AspNetCore.Mvc;

namespace MakiseSharpServer.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IDiscordApi discordClient;
        private readonly AppSettings appSettings;
        private readonly IDiscordJwtCreator jwtCreator;

        public TokenController(AppSettings appSettings, IDiscordApi discordClient, IDiscordJwtCreator jwtCreator)
        {
            this.appSettings = appSettings;
            this.discordClient = discordClient;
            this.jwtCreator = jwtCreator;
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
                switch (e.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        return BadRequest("Wrong access code.");
                    case HttpStatusCode.ServiceUnavailable:
                    case HttpStatusCode.InternalServerError:
                    case HttpStatusCode.NotFound:
                        return new StatusCodeResult(503);
                    default:
                        return new StatusCodeResult(500);
                }

                //ToDo: log it
                throw;
            }

            var user = await discordClient.GetBasicUserInfoAsync($"Bearer {token.AccessToken}");

            //ToDo: save access&refresh token to db

            return new ObjectResult(new JwtSecurityTokenHandler().WriteToken(jwtCreator.FromUser(user)));
        }
    }
}