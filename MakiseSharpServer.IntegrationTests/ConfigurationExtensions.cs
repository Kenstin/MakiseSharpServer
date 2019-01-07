using Microsoft.Extensions.Configuration;

namespace MakiseSharpServer.IntegrationTests
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration SetupTestConfiguration(this IConfiguration config)
        {
            config["discord:clientId"] = "0";
            config["discord:clientSecret"] = "abc";
            config["discord:redirectUri"] = "https://google.com";
            config["discord:apiUri"] = "https://google.com";
            config["token:audience"] = "audienceone";
            config["token:validAudiences:0"] = "audienceone";
            config["token:validAudiences:1"] = "audiencetwo";
            config["token:issuer"] = "issuerone";
            config["token:validIssuers:0"] = "issuerone";
            config["token:validIssuers:1"] = "issuertwo";
            config["token:signingKey"] = "averylongstringqwertyuiopasdfghjklzxcvbnm1234567890gsdgsgdssdfsddsstt";
            config["token:tokenLifetime"] = "5";
            config["token:claims:discordUserId"] = "https://dId.com";
            config["token:claims:discordUserDiscriminator"] = "https://dDiscriminator.com";
            config["token:claims:discordUserAvatar"] = "https://dAvatar.com";
            return config;
        }
    }
}
