using System.Collections.ObjectModel;

namespace ContainerProducts.Api.Core.DataAccess;

public abstract record QueryOperation
{
    public sealed record QueryNotFoundOperation : QueryOperation { }

    public sealed record QuerySingleOperation<T> : QueryOperation
    {
        private QuerySingleOperation(T data) => Data = data;

        public T Data { get; }

        public static QuerySingleOperation<T> New(T data) => new(data);
    }

    public sealed record QueryListOperation<T> : QueryOperation
    {
        private QueryListOperation(IEnumerable<T> records) =>
            Records = new ReadOnlyCollection<T>(records?.ToList() ?? new List<T>());

        public ReadOnlyCollection<T> Records { get; }

        public static QueryListOperation<T> New(IEnumerable<T> records) => new(records);
    }

    public sealed record QueryFailedOperation : QueryOperation
    {
        private QueryFailedOperation(int errorCode, string errorMessage, Exception? exception)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public int ErrorCode { get; }
        public string ErrorMessage { get; }
        public Exception? Exception { get; }

        public static QueryFailedOperation New(int errorCode, string errorMessage) =>
            new(errorCode, errorMessage, null);

        public static QueryFailedOperation New(
            int errorCode,
            string errorMessage,
            Exception exception
        ) => new(errorCode, errorMessage, exception);
    }
}
