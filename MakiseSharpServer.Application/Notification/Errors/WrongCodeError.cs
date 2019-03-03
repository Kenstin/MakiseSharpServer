using MakiseSharpServer.Common;

namespace MakiseSharpServer.Application.Notification.Errors
{
    public class WrongCodeError : Error
    {
        public WrongCodeError()
            : base("Wrong code provided.")
        {
        }
    }
}
