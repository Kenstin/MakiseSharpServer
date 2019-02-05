using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;

namespace MakiseSharpServer.Identity
{
    public static class DevConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var discordResource = new IdentityResource("discord", "Discord User Information",
                new List<string> { ClaimTypes.NameIdentifier, "urn:discord:avatar", "urn:discord:discriminator"});
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                discordResource
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            var makise = new ApiResource("makise", "MakiseSharp")
            {
                UserClaims = new List<string>
                {
                    "discord"
                }
            };
            return new List<ApiResource>
            {
                makise
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "dashboard",
                    ClientName = "Makise Dashboard",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false,
                    EnableLocalLogin = false,
                    RequireConsent = false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    

                    IdentityProviderRestrictions = { "Discord"},
                    PostLogoutRedirectUris = { "https://makise.club" },
                    RedirectUris =           { "https://makise.club/dashboard/login" },
                    AllowedCorsOrigins =     { "https://localhost:5000" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "discord",
                        "makise"
                    }
                }
            };
        }
    }
}
