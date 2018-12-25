using MakiseSharpServer.Common;

namespace MakiseSharpServer.Application.ApiClients.Errors
{
    public class ForbiddenError : Error
    {
        public ForbiddenError()
            : base("Request forbidden")
        {
        }
    }
}