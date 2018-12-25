using System.Collections.Generic;

namespace MakiseSharpServer.Common
{
    public class Result<T>
    {
        private readonly List<Error> errors;

        public Result(T data)
        {
            errors = new List<Error>();
            Data = data;
        }

        public Result(IEnumerable<Error> errors)
        {
            this.errors = new List<Error>(errors);
        }

        public Result(Error error)
        {
            errors = new List<Error> { error };
        }

        public IReadOnlyCollection<Error> Errors => errors.AsReadOnly();

        public bool IsSuccess => errors.Count == 0;

        public T Data { get; }
    }
}