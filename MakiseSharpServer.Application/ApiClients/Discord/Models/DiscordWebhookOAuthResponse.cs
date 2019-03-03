namespace MakiseSharpServer.Application.ApiClients.Discord.Models
{
    public class DiscordWebhookOAuthResponse
    {
        public DiscordWebhookOAuthResponse(DiscordWebhook webhook)
        {
            Webhook = webhook;
        }

        public DiscordWebhook Webhook { get; }
    }
}
