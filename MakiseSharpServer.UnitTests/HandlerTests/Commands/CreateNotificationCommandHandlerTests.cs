using System.Threading;
using System.Threading.Tasks;
using MakiseSharpServer.Application.Notification.Commands.CreateNotification;
using MakiseSharpServer.Application.Notification.Errors;
using MakiseSharpServer.Application.Notification.Models.StatusProviders;
using MakiseSharpServer.Domain.Entities.NotificationAggregate;
using Moq;
using Xunit;

namespace MakiseSharpServer.UnitTests.HandlerTests.Commands
{
    public class CreateNotificationCommandHandlerTests
    {
        private readonly CreateNotificationCommandHandler handler;
        private readonly Mock<INotificationRepository> repository;
        private readonly Mock<IStatusProviderFactory> providerFactory;
        private readonly CancellationToken cltToken;

        public CreateNotificationCommandHandlerTests()
        {
            repository = new Mock<INotificationRepository>();
            providerFactory = new Mock<IStatusProviderFactory>();
            cltToken = CancellationToken.None;
            handler = new CreateNotificationCommandHandler(repository.Object, providerFactory.Object);
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
    }
}