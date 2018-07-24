using System.Collections.Generic;

namespace MakiseSharpServer.Models.Settings
{
    public class TokenSettings
    {
        public IReadOnlyCollection<string> ValidAudiences { get; set; }
        public IReadOnlyCollection<string> ValidIssuers { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string SigningKey { get; set; }
        public uint TokenLifetime { get; set; }
        public ClaimsSettings Claims { get; set; }
    }
}