using System.Linq;
using System.Security.Claims;
using MakiseSharpServer.Models.Discord;
using MakiseSharpServer.Models.Settings;
using MakiseSharpServer.Services;
using Xunit;

namespace MakiseSharpServerTests.Controllers
{
    public class TokenControllerTests
    {
        private readonly DiscordUser user;
        private readonly JwtCreator jwtCreator;

        public TokenControllerTests()
        {
            user = new DiscordUser()
            {
                Id = 123,
                Avatar = "avatar",
                Discriminator = "discriminator",
                Username = "Test"
            };
            jwtCreator = new JwtCreator(new AppSettings()
            {
                Token = new TokenSettings()
                {
                    Claims = new ClaimsSettings()
                    {
                        DiscordUserAvatar = "AvatarClaim",
                        DiscordUserDiscriminator = "DiscriminatorClaim",
                        DiscordUserId = "IdClaim"
                    },
                    Audience = "aud",
                    Issuer = "iss",
                    SigningKey = "geigeoggegoewgtegiojoteiowetwotwewtwtwgrsgsgtgewigwhtweirwer2334324"
                }
            });
        }

        [Fact]
        public void ClaimsAreFilledWithUserInfo()
        {
            var token = jwtCreator.FromUser(user);
            
            Assert.Equal("123", token.Claims.First(claim => claim.Type == "IdClaim").Value);
            Assert.Equal("avatar", token.Claims.First(claim => claim.Type == "AvatarClaim").Value);
            Assert.Equal("discriminator", token.Claims.First(claim => claim.Type == "DiscriminatorClaim").Value);
            Assert.Equal("Test", token.Claims.First(claim => claim.Type == ClaimTypes.Name).Value);
        }
    }
}
