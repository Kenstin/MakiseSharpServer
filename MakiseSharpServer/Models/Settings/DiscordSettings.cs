﻿using System;

namespace MakiseSharpServer.Models.Settings
{
    public class DiscordSettings
    {
        public ulong ClientId { get; set; }

        public string ClientSecret { get; set; }

        public Uri RedirecUri { get; set; }
    }
}