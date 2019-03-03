using System;
using Refit;

namespace MakiseSharpServer.Application.Notification.DTOs
{
    public class ExchangeCodeForDiscordWebhookDto
    {
        public ExchangeCodeForDiscordWebhookDto(ulong clientId, string clientSecret, string accessCode, Uri redirectUri)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            AccessCode = accessCode;
            RedirectUri = redirectUri;
        }

        [AliasAs("client_id")]
        public ulong ClientId { get; }

        [AliasAs("client_secret")]
        public string ClientSecret { get; }

        [AliasAs("grant_type")]
        public string GrantType => "authorization_code";

        [AliasAs("code")]
        public string AccessCode { get; }

        [AliasAs("redirect_uri")]
        public Uri RedirectUri { get; }

        public string Scope => "webhook.incoming";
    }
}