using MakiseSharpServer.Common;

namespace MakiseSharpServer.Application.Notification.Errors
{
    public class StatusProviderNotSupportedError : Error
    {
        public StatusProviderNotSupportedError()
            : base("Requested status provider is not supported.")
        {
        }
    }
}
