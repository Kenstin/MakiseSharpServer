using System;

namespace MakiseSharpServer.API.Models.Settings
{
    public class DiscordSettings
    {
        public ulong ClientId { get; set; }

        public string ClientSecret { get; set; }

        public Uri RedirecUri { get; set; }

        public Uri ApiUri { get; set; }
    }
}