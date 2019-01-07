using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MakiseSharpServer.Application.ApiClients.DiscordApi;
using MakiseSharpServer.Application.Authentication;
using MakiseSharpServer.Application.Authentication.Models;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Refit;
using Xunit;

namespace MakiseSharpServer.IntegrationTests.ControllerTests
{
    public class TokenControllerTests : IClassFixture<EfWebApplicationFactory>
    {
        private readonly EfWebApplicationFactory factory;

        public TokenControllerTests(EfWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task ReturnsOkWhenAuthorizationSucceeds()
        {
            //Arrange
            var client = GetClientWithDiscordApiBehavior(provider => Mock.Of<IDiscordApi>(api =>
                api.GetAccessTokenAsync(It.IsAny<DiscordAccessTokenRequestDto>()) == Task.FromResult(new DiscordToken()
                {
                    AccessToken = "abc",
                    RefreshToken = "xyz"
                })
                && api.GetBasicUserInfoAsync(It.IsAny<string>()) == Task.FromResult(new DiscordUser(
                    default(ulong), string.Empty, string.Empty, string.Empty))));

            //Act
            var response = await client.PostAsync("/api/token?accessToken=test", null);

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ReturnsBadRequestWhenNoAccessToken()
        {
            //Arrange
            var client = factory.CreateClient();

            //Act
            var response = await client.PostAsync("/api/token", null);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ReturnsUnauthorizedWhenWrongAccessToken()
        {
            //Arrange
            var refitException = await GetRefitException(HttpStatusCode.Unauthorized);
            var client = GetClientWithDiscordApiBehavior(provider =>
            {
                var api = new Mock<IDiscordApi>();
                api.Setup(setup => setup.GetAccessTokenAsync(It.IsAny<DiscordAccessTokenRequestDto>()))
                    .ThrowsAsync(refitException);
                return api.Object;
            });

            //Act
            var response = await client.PostAsync("/api/token?accessToken=wrong", null);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ReturnsUnavailableWhenDiscordApiUnavailable()
        {
            //Arrange
            var refitException = await GetRefitException(HttpStatusCode.ServiceUnavailable);
            var client = GetClientWithDiscordApiBehavior(provider =>
            {
                var api = new Mock<IDiscordApi>();
                api.Setup(setup => setup.GetAccessTokenAsync(It.IsAny<DiscordAccessTokenRequestDto>()))
                    .ThrowsAsync(refitException);
                return api.Object;
            });

            //Act
            var response = await client.PostAsync("/api/token?accessToken=xyz", null);

            //Assert
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        }

        private static async Task<ApiException> GetRefitException(HttpStatusCode statusCode) =>
            await ApiException.Create(null, null, new HttpResponseMessage(statusCode));

        private HttpClient GetClientWithDiscordApiBehavior(Func<IServiceProvider, IDiscordApi> apiBehavior)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => { services.AddScoped(apiBehavior); });
            }).CreateClient();
        }
    }
}
