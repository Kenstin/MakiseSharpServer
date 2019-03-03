using MakiseSharpServer.Application.Notification.Commands.CreateNotification;

namespace MakiseSharpServer.Application.Notification.Models.StatusProviders
{
    public interface IStatusProvider
    {
        bool DoesSupport(string statusProvider);
    }
}
