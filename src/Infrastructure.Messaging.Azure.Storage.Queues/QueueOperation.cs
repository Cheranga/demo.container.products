namespace Infrastructure.Messaging.Azure.Storage.Queues;

public abstract record QueueOperation
{
    private QueueOperation() { }

    public sealed record FailedOperation : QueueOperation
    {
        public int ErrorCode { get; }
        public string ErrorMessage { get; }
        public Exception? Exception { get; }

        private FailedOperation(int errorCode, string errorMessage, Exception? exception)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public static FailedOperation New(int errorCode, string errorMessage) =>
            new(errorCode, errorMessage, null);

        public static FailedOperation New(
            int errorCode,
            string errorMessage,
            Exception exception
        ) => new(errorCode, errorMessage, exception);
    }

    public record SuccessOperation : QueueOperation
    {
        private SuccessOperation() { }

        public static SuccessOperation New() => new();
    }
}
