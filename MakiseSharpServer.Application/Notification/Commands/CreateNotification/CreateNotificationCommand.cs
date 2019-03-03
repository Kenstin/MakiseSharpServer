using MakiseSharpServer.Application.Notification.DTOs;
using MakiseSharpServer.Common;
using MediatR;

namespace MakiseSharpServer.Application.Notification.Commands.CreateNotification
{
    public class CreateNotificationCommand : IRequest<Result<NotificationCreatedDto>>
    {
        public string DiscordWebhookOAuthCode { get; }

        public string StatusProvider { get; }
    }
}
