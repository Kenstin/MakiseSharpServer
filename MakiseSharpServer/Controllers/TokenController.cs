using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.APIs;
using ServiceLayer.JwtServices;
using ServiceLayer.Models.Discord;
using ServiceLayer.Models.Jwt;
using ServiceLayer.Settings;

namespace MakiseSharpServer.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IDiscordApi discordClient;
        private readonly AppSettings appSettings;
        private readonly IDiscordJwtCreator jwtCreator;
        private readonly ITokenFactory tokenFactory;

        public TokenController(AppSettings appSettings, IDiscordApi discordClient, IDiscordJwtCreator jwtCreator, ITokenFactory tokenFactory)
        {
            this.appSettings = appSettings;
            this.discordClient = discordClient;
            this.jwtCreator = jwtCreator;
            this.tokenFactory = tokenFactory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenAsync(string code, [FromServices] ISetTokensForUserService service)
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

            var appRefreshToken = tokenFactory.GenerateToken();
            await service.SetTokensAsync(user.Id, token.RefreshToken, token.AccessToken, appRefreshToken);

            return Ok(new JwtResponse(
                new AccessToken(
                    new JwtSecurityTokenHandler().WriteToken(jwtCreator.FromUser(user)),
                    appSettings.Token.TokenLifetime), appRefreshToken));
        }
    }
}