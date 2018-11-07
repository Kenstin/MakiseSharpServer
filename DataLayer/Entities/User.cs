using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public DiscordValues DiscordValues { get; set; }
 
        public ICollection<RefreshToken> AppRefreshTokens { get; set; }
    }
}
