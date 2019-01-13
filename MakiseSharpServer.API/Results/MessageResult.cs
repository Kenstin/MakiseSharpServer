using Microsoft.AspNetCore.Mvc;

namespace MakiseSharpServer.API.Results
{
    public class MessageResult : ObjectResult
    {
        public MessageResult(int statusCode, string message)
            : base(new { Message = message })
        {
            StatusCode = statusCode;
        }
    }
}
