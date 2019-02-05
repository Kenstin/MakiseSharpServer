using System;

namespace MakiseSharpServer.Application.Settings
{
    public class DiscordSettings
    {
        public ulong ClientId { get; set; }

        public string ClientSecret { get; set; }

        public Uri RedirectUri { get; set; }

        public Uri ApiUri { get; set; }

        public Uri CdnUri { get; set; }
    }
}