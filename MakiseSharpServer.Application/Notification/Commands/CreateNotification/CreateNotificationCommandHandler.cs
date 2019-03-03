using System;
using System.Threading;
using System.Threading.Tasks;
using MakiseSharpServer.Application.Notification.DTOs;
using MakiseSharpServer.Application.Notification.Errors;
using MakiseSharpServer.Application.Notification.Models.StatusProviders;
using MakiseSharpServer.Common;
using MakiseSharpServer.Domain.Entities.NotificationAggregate;
using MediatR;

namespace MakiseSharpServer.Application.Notification.Commands.CreateNotification
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Result<NotificationCreatedDto>>
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IStatusProviderFactory statusProviderFactory;
        private IStatusProvider statusProvider;

        public CreateNotificationCommandHandler(INotificationRepository notificationRepository, IStatusProviderFactory statusProviderFactory)
        {
            this.notificationRepository = notificationRepository;
            this.statusProviderFactory = statusProviderFactory;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<Result<NotificationCreatedDto>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            statusProvider = statusProviderFactory.GetStatusProviderForCommand(request);
            if (statusProvider == null)
            {
                return new StatusProviderNotSupportedError().AsResult<NotificationCreatedDto>();
            }

            throw new NotImplementedException();
        }
    }
}
