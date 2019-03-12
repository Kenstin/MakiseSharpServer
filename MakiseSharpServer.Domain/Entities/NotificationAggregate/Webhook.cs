using System;
using System.Collections.Generic;
using MakiseSharpServer.Domain.SeedWork;

namespace MakiseSharpServer.Domain.Entities.NotificationAggregate
{
    public class Webhook : ValueObject
    {
        public Webhook(ulong webhookDiscordId, string webhookDiscordToken, ulong channelId, ulong guildId)
        {
            WebhookDiscordId = webhookDiscordId;
            WebhookDiscordToken = webhookDiscordToken ?? throw new ArgumentNullException(nameof(webhookDiscordToken));
            ChannelId = channelId;
            GuildId = guildId;
        }

        public ulong WebhookDiscordId { get; private set; }

        public string WebhookDiscordToken { get; private set; }

        public ulong ChannelId { get; private set; }

        public ulong GuildId { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return WebhookDiscordId;
            yield return WebhookDiscordToken;
            yield return ChannelId;
            yield return GuildId;
        }
    }
}
