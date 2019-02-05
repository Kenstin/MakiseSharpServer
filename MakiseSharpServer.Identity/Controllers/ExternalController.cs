using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Services;
using MakiseSharpServer.Domain.Entities.UserAggregate;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MakiseSharpServer.Identity.Controllers
{
    [AllowAnonymous]
    public class ExternalController : Controller
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly IEventService events;
        private readonly IUserRepository userRepository;

        public ExternalController(
            IIdentityServerInteractionService interaction,
            IEventService events,
            IUserRepository userRepository)
        {
            this.interaction = interaction;
            this.events = events;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public IActionResult Challenge(string provider, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "~/";
            }

            // validate returnUrl - either it is a valid OIDC URL or back to a local page
            if (Url.IsLocalUrl(returnUrl) == false && interaction.IsValidReturnUrl(returnUrl) == false)
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("Invalid return URL");
            }


            // start challenge and roundtrip the return URL and scheme 
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback)),
                Items =
                {
                    {"returnUrl", returnUrl},
                    {"scheme", provider},
                }
            };

            return Challenge(props, provider);
        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            // lookup our user and external provider info
            var (user, provider, providerUserId, claims) = await FindUserFromExternalProvider(result);
            if (user == null)
            {
                user = await CreateUser(provider, providerUserId, result.Properties);
            }

            var username = claims.First(c => c.Type == ClaimTypes.Name).Value;

            // issue authentication cookie for user
            await events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id.ToString(), username));
            await HttpContext.SignInAsync(user.Id.ToString(), username, provider, claims.ToArray());

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // retrieve return URL
            var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

            return Redirect(returnUrl);
        }

        private async Task<User> CreateUser(string provider, string providerUserId, AuthenticationProperties authProperties)
        {
            if (provider != "Discord")
            {
                throw new NotSupportedException("Providers other than Discord are not supported.");
            }

            var user = new User(ulong.Parse(providerUserId), authProperties.GetTokenValue("access_token"), 
                authProperties.GetTokenValue("refresh_token"));
            await userRepository.AddAsync(user);
            await userRepository.UnitOfWork.SaveEntitiesAsync();

            return user;
        }


        private async Task<(User user, string provider, string providerUserId, ICollection<Claim> claims)> FindUserFromExternalProvider(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            var providerUserIdClaim = externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");

            var claims = externalUser.Claims.ToList();

            var provider = result.Properties.Items["scheme"];
            var providerUserId = providerUserIdClaim.Value;

            // find external user
            var user = await userRepository.GetByDiscordId(ulong.Parse(providerUserId));

            return (user, provider, providerUserId, claims);
        }
    }
}