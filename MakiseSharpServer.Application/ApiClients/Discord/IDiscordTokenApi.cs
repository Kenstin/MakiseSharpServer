using System.Threading.Tasks;
using MakiseSharpServer.Application.ApiClients.Discord.Models;
using MakiseSharpServer.Application.Notification.DTOs;
using Refit;

namespace MakiseSharpServer.Application.ApiClients.Discord
{
    public interface IDiscordTokenApi
    {
        [Post("/oauth2/token")]
        Task<DiscordWebhookOAuthResponse> GetWebhookAsync([Body(BodySerializationMethod.UrlEncoded)]
            ExchangeCodeForDiscordWebhookDto exchangeCodeForDiscordWebhookDto);
    }
}
