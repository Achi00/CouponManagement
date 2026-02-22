namespace CouponApp.Application
{
    public class Result
    {
        public bool Succeeded { get; }
        public IEnumerable<string> Errors { get; }

        private Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public static Result Success()
            => new Result(true, Array.Empty<string>());

        public static Result Failure(IEnumerable<string> errors)
            => new Result(false, errors);

        public static Result Failure(string error)
            => new Result(false, new[] { error });
    }

    public class Result<T>
    {
        public bool Succeeded { get; }
        public IEnumerable<string> Errors { get; }
        public T Value { get; }

        private Result(bool succeeded, T value, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Value = value;
            Errors = errors;
        }

        public static Result<T> Success(T value)
            => new Result<T>(true, value, Array.Empty<string>());

        public static Result<T> Failure(IEnumerable<string> errors)
            => new Result<T>(false, default!, errors);

        public static Result<T> Failure(string error)
            => new Result<T>(false, default!, new[] { error });
    }

}
