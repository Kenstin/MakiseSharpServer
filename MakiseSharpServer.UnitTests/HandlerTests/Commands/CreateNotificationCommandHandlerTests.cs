using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MakiseSharpServer.Application.ApiClients.Discord;
using MakiseSharpServer.Application.Notification.Commands.CreateNotification;
using MakiseSharpServer.Application.Notification.DTOs;
using MakiseSharpServer.Application.Notification.Errors;
using MakiseSharpServer.Application.Notification.Models.StatusProviders;
using MakiseSharpServer.Application.Settings;
using MakiseSharpServer.Domain.Entities.NotificationAggregate;
using Moq;
using Refit;
using Xunit;

namespace MakiseSharpServer.UnitTests.HandlerTests.Commands
{
    public class CreateNotificationCommandHandlerTests
    {
        private readonly CreateNotificationCommandHandler handler;
        private readonly Mock<INotificationRepository> repository;
        private readonly Mock<IStatusProviderFactory> providerFactory;
        private readonly Mock<IDiscordTokenApi> discordTokenApi;
        private readonly CancellationToken cltToken;

        public CreateNotificationCommandHandlerTests()
        {
            repository = new Mock<INotificationRepository>();
            providerFactory = new Mock<IStatusProviderFactory>();
            discordTokenApi = new Mock<IDiscordTokenApi>();
            cltToken = CancellationToken.None;
            var settings = new AppSettings()
            {
                Discord = new DiscordSettings()
            };
            handler = new CreateNotificationCommandHandler(repository.Object, providerFactory.Object, discordTokenApi.Object, settings);
        }

        [Fact]
        public async Task ProviderNotSupportedErrorWhenStatusProviderNotSupported()
        {
            //Arrange
            providerFactory.Setup(f => f.GetStatusProviderForCommand(It.IsAny<CreateNotificationCommand>()))
                .Returns((IStatusProvider) null);

            //Act
            var result = await handler.Handle(new CreateNotificationCommand(), cltToken);

            //Assert
            Assert.Contains(result.Errors, e => e is StatusProviderNotSupportedError);
        }

        [Fact]
        public async Task WrongTokenErrorGivenWrongOAuthToken()
        {
            //Arrange
            var statusProviderMock = new Mock<IStatusProvider>();
            providerFactory.Setup(f => f.GetStatusProviderForCommand(It.IsAny<CreateNotificationCommand>()))
                .Returns(statusProviderMock.Object);
            discordTokenApi.Setup(a => a.GetWebhookAsync(It.IsAny<ExchangeCodeForDiscordWebhookDto>()))
                .ThrowsAsync(await GetRefitException(HttpStatusCode.Unauthorized));

            //Act
            var result = await handler.Handle(new CreateNotificationCommand(), cltToken);

            //Assert
            Assert.Contains(result.Errors, e => e is WrongCodeError);
        }

        [Theory]
        [ClassData(typeof(UnavailableStatusCodesTestData))]
        public async Task UnavailableErrorWhenDiscordApiRequestFailed(HttpStatusCode statusCode)
        {
            //Arrange
            var statusProviderMock = new Mock<IStatusProvider>();
            providerFactory.Setup(f => f.GetStatusProviderForCommand(It.IsAny<CreateNotificationCommand>()))
                .Returns(statusProviderMock.Object);
            discordTokenApi.Setup(a => a.GetWebhookAsync(It.IsAny<ExchangeCodeForDiscordWebhookDto>()))
                .ThrowsAsync(await GetRefitException(statusCode));
            //Act
            var result = await handler.Handle(new CreateNotificationCommand(), cltToken);

            //Assert
            Assert.Contains(result.Errors, e => e is UnavailableError);
        }

        private static async Task<ApiException> GetRefitException(HttpStatusCode statusCode) =>
            await ApiException.Create(null, null, new HttpResponseMessage(statusCode));
    }
}