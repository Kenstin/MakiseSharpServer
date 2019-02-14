using Microsoft.Extensions.Configuration;

namespace MakiseSharpServer.IntegrationTests
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration SetupTestConfiguration(this IConfiguration config)
        {
            config["discord:clientId"] = "0";
            config["discord:clientSecret"] = "abc";
            config["discord:apiUri"] = "https://google.com";
            config["token:validAudiences:0"] = "audienceone";
            config["token:validAudiences:1"] = "audiencetwo";
            config["token:validIssuers:0"] = "issuerone";
            config["token:validIssuers:1"] = "issuertwo";
            config["token:claims:discordUserId"] = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            config["token:claims:discordUserDiscriminator"] = "urn:discord:discriminator";
            config["token:claims:discordUserAvatar"] = "urn:discord:avatar";
            return config;
        }
    }
}
