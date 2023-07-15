namespace ContainerProducts.Api.Core.DataAccess;

public abstract record CommandOperation
{
    private CommandOperation() { }

    public static CommandSuccessOperation Success() => CommandSuccessOperation.New();

    public static CommandFailedOperation Failure(int errorCode, string errorMessage) =>
        CommandFailedOperation.New(errorCode, errorMessage);

    public static CommandFailedOperation Failure(
        int errorCode,
        string errorMessage,
        Exception exception
    ) => CommandFailedOperation.New(errorCode, errorMessage, exception);

    public sealed record CommandSuccessOperation : CommandOperation
    {
        private CommandSuccessOperation() { }

        public static CommandSuccessOperation New() => new();
    }

    public sealed record CommandFailedOperation : CommandOperation
    {
        private CommandFailedOperation(int errorCode, string errorMessage, Exception? exception)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public int ErrorCode { get; }
        public string ErrorMessage { get; }
        public Exception? Exception { get; }

        public static CommandFailedOperation New(int errorCode, string errorMessage) =>
            new(errorCode, errorMessage, null);

        public static CommandFailedOperation New(
            int errorCode,
            string errorMessage,
            Exception exception
        ) => new(errorCode, errorMessage, exception);
    }
}
