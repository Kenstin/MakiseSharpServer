namespace MakiseSharpServer.Common
{
    public class Error
    {
        public Error(string message)
        {
            Message = message;
        }

        public string Message { get; }

        public Result<T> AsResult<T>() => new Result<T>(this);
    }
}