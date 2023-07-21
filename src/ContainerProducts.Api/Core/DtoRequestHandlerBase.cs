using ContainerProducts.Api.Core.Domain;
using FluentValidation;

namespace ContainerProducts.Api.Core;

public interface IRequestHandler<in TRequest>
{
    Task<DR> ExecuteAsync(TRequest request, CancellationToken token);
}

public interface IRequestHandler<in TRequest, TResponse>
{
    Task<
        DomainResponse<
            DomainOperation.ValidationFailedOperation,
            DomainOperation.FailedOperation,
            DomainOperation.SuccessOperation<TResponse>
        >
    > HandleAsync(TRequest request, CancellationToken token);
}

public interface IRequest<TRequest, TValidator, THandler>
    where TRequest : class, IRequest<TRequest, TValidator, THandler>
    where TValidator : ModelValidatorBase<TRequest>, IValidator<TRequest>
    where THandler : IRequestHandler<TRequest>
{
    public string CorrelationId { get; set; }
}

public interface IRequest<TRequest, TResponse, TValidator, THandler>
    where TRequest : class, IRequest<TRequest, TResponse, TValidator, THandler>
    where TResponse : class
    where TValidator : ModelValidatorBase<TRequest>, IValidator<TRequest>
    where THandler : IRequestHandler<TRequest>
{
    public string CorrelationId { get; set; }
}
