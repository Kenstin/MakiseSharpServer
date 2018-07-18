using System.Collections.Generic;

namespace MakiseSharpServer.Models.Settings
{
    public class TokenSettings
    {
        public IReadOnlyCollection<string> ValidAudiences { get; set; }
        public IReadOnlyCollection<string> ValidIssuers { get; set; }
        public string SigningKey { get; set; }
        public string TokenLifetime { get; set; }
    }
}