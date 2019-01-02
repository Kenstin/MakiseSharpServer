using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;

namespace MakiseSharpServer.Common
{
    public static class HttpStatusCodesExtensions
    {
        public static ImmutableList<HttpStatusCode> UnavailableCodes { get; } = ImmutableList.CreateRange(new List<HttpStatusCode>()
        {
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.GatewayTimeout
        });

        public static bool IsUnavailable(this HttpStatusCode code) => UnavailableCodes.Contains(code);
    }
}