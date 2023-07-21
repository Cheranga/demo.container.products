using FluentValidation.Results;

namespace ContainerProducts.Api.Core.Domain;

public abstract record DomainOperation
{
    private DomainOperation() { }

    public sealed record SuccessOperation : DomainOperation
    {
        private SuccessOperation() { }

        internal static SuccessOperation New() => new();
    }

    public sealed record SuccessOperation<T> : DomainOperation
    {
        public T Data { get; }

        private SuccessOperation(T data)
        {
            Data = data;
        }

        public static SuccessOperation<T> New(T data) => new(data);
    }

    public sealed record ValidationFailedOperation : DomainOperation
    {
        public ValidationResult Failure { get; }

        private ValidationFailedOperation(ValidationResult failure)
        {
            Failure = failure;
        }

        public static ValidationFailedOperation New(ValidationResult failure) => new(failure);
    }

    public sealed record FailedOperation : DomainOperation
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

        public static FailedOperation New(
            int errorCode,
            string errorMessage,
            Exception? exception
        ) => new(errorCode, errorMessage, exception);
    }
}
