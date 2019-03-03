using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MakiseSharpServer.Application.ApiClients.Discord;
using MakiseSharpServer.Application.ApiClients.Discord.Models;
using MakiseSharpServer.Application.Notification.DTOs;
using MakiseSharpServer.Application.Notification.Errors;
using MakiseSharpServer.Application.Notification.Models.StatusProviders;
using MakiseSharpServer.Application.Settings;
using MakiseSharpServer.Common;
using MakiseSharpServer.Domain.Entities.NotificationAggregate;
using MediatR;

namespace MakiseSharpServer.Application.Notification.Commands.CreateNotification
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Result<NotificationCreatedDto>>
    {
        private readonly IDiscordTokenApi discordApi;
        private readonly INotificationRepository notificationRepository;
        private readonly IStatusProviderFactory statusProviderFactory;
        private readonly AppSettings appSettings;
        private IStatusProvider statusProvider;

        public CreateNotificationCommandHandler(INotificationRepository notificationRepository, IStatusProviderFactory statusProviderFactory, IDiscordTokenApi discordApi, AppSettings appSettings)
        {
            this.notificationRepository = notificationRepository;
            this.statusProviderFactory = statusProviderFactory;
            this.discordApi = discordApi;
            this.appSettings = appSettings;
        }

        public async Task<Result<NotificationCreatedDto>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            statusProvider = statusProviderFactory.GetStatusProviderForCommand(request);
            if (statusProvider == null)
            {
                return new StatusProviderNotSupportedError().AsResult<NotificationCreatedDto>();
            }

            DiscordWebhookOAuthResponse response;
            try
            {
                response = await discordApi.GetWebhookAsync(new ExchangeCodeForDiscordWebhookDto(
                    appSettings.Discord.ClientId,
                    appSettings.Discord.ClientSecret,
                    request.DiscordWebhookOAuthCode,
                    appSettings.Discord.WebhookCreationRedirectUri));
            }
            catch (Refit.ApiException e)
            {
                switch (e.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        return new WrongCodeError().AsResult<NotificationCreatedDto>();
                    default:
                        throw;
                }
            }

            throw new NotImplementedException();
        }
    }
}
