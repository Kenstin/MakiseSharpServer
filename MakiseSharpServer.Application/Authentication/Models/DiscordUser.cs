using System;

namespace MakiseSharpServer.Application.Authentication.Models
{
    public class DiscordUser
    {
        private readonly string avatarHash;

        public DiscordUser(ulong id, string username, string discriminator, string avatarHash)
        {
            Id = id;
            Username = username;
            Discriminator = discriminator;
            this.avatarHash = avatarHash;
        }

        public ulong Id { get; }

        public string Username { get; }

        public string Discriminator { get; }

        public Uri GetAvatarLink(Uri discordApi)
        {
            return new Uri(discordApi, $"avatars/{Id}/{avatarHash}.png");
        }
    }
}
