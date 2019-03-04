using MakiseSharpServer.Common;

namespace MakiseSharpServer.Application.Notification.Errors
{
    public class UnavailableError : Error
    {
        public UnavailableError()
            : base("This resource is unavailable. Try again later.")
        {
        }
    }
}
