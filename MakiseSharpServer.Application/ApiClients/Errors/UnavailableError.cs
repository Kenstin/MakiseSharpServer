using MakiseSharpServer.Common;

namespace MakiseSharpServer.Application.ApiClients.Errors
{
    public class UnavailableError : Error
    {
        public UnavailableError()
            : base("This resource is unavailable. Try again later.")
        {
        }
    }
}