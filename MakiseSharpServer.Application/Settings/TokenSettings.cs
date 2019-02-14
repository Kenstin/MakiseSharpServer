using System.Collections.Generic;

namespace MakiseSharpServer.Application.Settings
{
    public class TokenSettings
    {
        public IReadOnlyCollection<string> ValidAudiences { get; set; }

        public IReadOnlyCollection<string> ValidIssuers { get; set; }

        public ClaimsSettings Claims { get; set; }
    }
}