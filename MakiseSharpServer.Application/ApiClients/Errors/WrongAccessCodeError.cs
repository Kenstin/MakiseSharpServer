using MakiseSharpServer.Common;

namespace MakiseSharpServer.Application.ApiClients.Errors
{
    public class WrongAccessCodeError : Error
    {
        public WrongAccessCodeError()
            : base("Wrong access code provided.")
        {
        }
    }
}