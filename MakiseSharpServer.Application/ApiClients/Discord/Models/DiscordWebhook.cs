using System;

namespace MakiseSharpServer.Application.ApiClients.Discord.Models
{
    public class DiscordWebhook
    {
        public DiscordWebhook(ulong id, string name, Uri url, ulong channelId, string token, string avatar, ulong guildId)
        {
            Id = id;
            Name = name;
            Url = url;
            ChannelId = channelId;
            Token = token;
            Avatar = avatar;
            GuildId = guildId;
        }

        public ulong Id { get; }

        public string Name { get; }

        public Uri Url { get; }

        public ulong ChannelId { get; }

        public string Token { get; }

        public string Avatar { get; }

        public ulong GuildId { get; }
    }
}
