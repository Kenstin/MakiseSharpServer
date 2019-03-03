using MakiseSharpServer.Application.Notification.Commands.CreateNotification;

namespace MakiseSharpServer.Application.Notification.Models.StatusProviders
{
    public interface IStatusProviderFactory
    {
        IStatusProvider GetStatusProviderForCommand(CreateNotificationCommand command);
    }
}
