using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MakiseSharpServer.Application.ApiClients.DiscordApi;
using MakiseSharpServer.Application.Authentication;
using MakiseSharpServer.Application.Authentication.Commands.CreateToken;
using MakiseSharpServer.Application.Authentication.Models;
using MakiseSharpServer.Application.Authentication.Services;
using MakiseSharpServer.Application.Settings;
using MakiseSharpServer.Common;
using MakiseSharpServer.Domain.Entities.UserAggregate;
using Moq;
using Refit;
using Xunit;

namespace MakiseSharpServer.UnitTests.Application.Authentication
{
    public class CreateTokenCommandHandlerTests
    {
        private readonly Mock<IDiscordApi> discordApi;
        private readonly Mock<IDiscordJwtCreator> jwtCreator;
        private readonly Mock<IUserRepository> userRepository;
        private readonly CancellationToken cltToken;
        private readonly CreateTokenCommandHandler handler;

        public CreateTokenCommandHandlerTests()
        {
            discordApi = new Mock<IDiscordApi>();
            jwtCreator = new Mock<IDiscordJwtCreator>();
            userRepository = new Mock<IUserRepository>();
            var appSettings = new AppSettings {Token = new TokenSettings {TokenLifetime = default(uint)}};
            cltToken = CancellationToken.None;
            handler = new CreateTokenCommandHandler(discordApi.Object, jwtCreator.Object, userRepository.Object, appSettings);  
        }

        [Fact]
        public async Task FailsWhenGetAccessTokenReturnsError()
        {
            foreach (var code in HttpStatusCodesExtensions.UnavailableCodes.Add(HttpStatusCode.Unauthorized))
            {
                //Arrange
                discordApi.Setup(api => api.GetAccessTokenAsync(It.IsAny<DiscordAccessTokenRequestDto>()))
                    .ThrowsAsync(await GetRefitException(code));

                //Act
                var result = await handler.Handle(new CreateTokenCommand(null, default(ulong), null, null), cltToken);

                //Assert
                Assert.False(result.IsSuccess);
            }
        }

        [Fact]
        public async Task FailsWhenGetBasicInfoReturnsError()
        {
            foreach (var code in HttpStatusCodesExtensions.UnavailableCodes.AddRange(new List<HttpStatusCode>
            {
                HttpStatusCode.Unauthorized,
                HttpStatusCode.Forbidden
            }))
            {
                //Arrange
                discordApi.Setup(api => api.GetAccessTokenAsync(It.IsAny<DiscordAccessTokenRequestDto>()))
                    .ReturnsAsync(new DiscordToken());
                discordApi.Setup(api => api.GetBasicUserInfoAsync(It.IsAny<string>()))
                    .ThrowsAsync(await GetRefitException(code));

                //Act
                var result = await handler.Handle(new CreateTokenCommand(null, default(ulong), null, null), cltToken);

                //Assert
                Assert.False(result.IsSuccess);
            }
        }

        [Fact]
        public async Task AddsUserToDbWhenUserNew()
        {
            const string accessToken = "xyz";
            const string refreshToken = "cvb";

            //Arrange
            discordApi.Setup(api => api.GetAccessTokenAsync(It.IsAny<DiscordAccessTokenRequestDto>()))
                .ReturnsAsync(new DiscordToken
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });
            discordApi.Setup(api => api.GetBasicUserInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(new DiscordUser(default(ulong), null, null, null));

            userRepository.Setup(repo => repo.GetByDiscordId(It.IsAny<ulong>()))
                .ReturnsAsync((User) null);
            userRepository.Setup(repo => repo.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            userRepository.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .ReturnsAsync(new User(default(ulong), accessToken, refreshToken));

            jwtCreator.Setup(creator => creator.FromUser(It.IsAny<DiscordUser>()))
                .Returns(new JwtSecurityToken());

            //Act
            var result = await handler.Handle(new CreateTokenCommand(null, default(ulong), null, null), cltToken);

            //Assert
            userRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()));
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ChangeUserCredentialsWhenUserExisting()
        {
            const string accessToken = "xyz";
            const string refreshToken = "cvb";
            const string newAccessToken = "qwe";
            const string newRefreshToken = "rty";

            //Arrange
            discordApi.Setup(api => api.GetAccessTokenAsync(It.IsAny<DiscordAccessTokenRequestDto>()))
                .ReturnsAsync(new DiscordToken
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            discordApi.Setup(api => api.GetBasicUserInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(new DiscordUser(default(ulong), null, null, null));
            
            var user = new User(default(ulong), accessToken, refreshToken);

            userRepository.Setup(repo => repo.GetByDiscordId(It.IsAny<ulong>()))
                .ReturnsAsync(user);
            userRepository.Setup(repo => repo.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            jwtCreator.Setup(creator => creator.FromUser(It.IsAny<DiscordUser>()))
                .Returns(new JwtSecurityToken());

            //Act
            var result = await handler.Handle(new CreateTokenCommand(null, default(ulong), null, null), cltToken);

            //Assert
            Assert.Equal(newAccessToken, user.DiscordAccessToken); //verify credentials changed
            Assert.Equal(newRefreshToken, user.DiscordRefreshToken);
            Assert.True(result.IsSuccess);
        }

        private static async Task<ApiException> GetRefitException(HttpStatusCode statusCode) =>
            await ApiException.Create(null, null, new HttpResponseMessage(statusCode));
    }
}
