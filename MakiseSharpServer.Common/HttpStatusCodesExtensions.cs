using System.Net;

namespace MakiseSharpServer.Common
{
    public static class HttpStatusCodesExtensions
    {
        public static bool IsUnavailable(this HttpStatusCode code)
        {
            switch (code)
            {
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.RequestTimeout:
                case HttpStatusCode.GatewayTimeout:
                    return true;
            }

            return false;
        }
    }
}