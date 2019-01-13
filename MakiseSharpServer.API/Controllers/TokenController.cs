using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MakiseSharpServer.API.Results;
using MakiseSharpServer.Application.ApiClients.Errors;
using MakiseSharpServer.Application.Authentication.Commands.CreateToken;
using MakiseSharpServer.Application.Authentication.Models;
using MakiseSharpServer.Application.Settings;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MakiseSharpServer.API.Controllers
{
    public class TokenController : BaseController
    {
        private readonly AppSettings appSettings;

        public TokenController(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        /// <summary>
        /// Used to create a new JWT and a refresh token given a correct Discord API access token
        /// </summary>
        /// <param name="accessToken">Discord API access token</param>
        /// <returns>JWT and refresh token</returns>
        [HttpPost]
        [SwaggerResponse(200, "Returns JWT with refresh token.")]
        [SwaggerResponse(401, "Wrong access code.")]
        [SwaggerResponse(500)]
        [SwaggerResponse(503, "Discord API is unavailable.")]
        public async Task<ActionResult<JwtResponse>> CreateTokenAsync([Required] string accessToken)
        {
            var result = await Mediator.Send(new CreateTokenCommand(accessToken, appSettings.Discord.ClientId, appSettings.Discord.ClientSecret, appSettings.Discord.RedirectUri));
            if (result.IsSuccess)
            {
                return result.Data;
            }

            if (result.Errors.Any(e => e is UnavailableError))
            {
                return new MessageResult(503, "Discord API is unavailable.");
            }

            if (result.Errors.Any(e => e is WrongAccessCodeError || e is ForbiddenError))
            {
                return new MessageResult(401, "Wrong access code.");
            }

            return new StatusCodeResult(500);
        }
    }
}
