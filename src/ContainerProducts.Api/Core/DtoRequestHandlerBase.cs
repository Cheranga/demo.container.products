using FluentValidation;

namespace ContainerProducts.Api.Core;

public interface IDtoRequest<TRequest, TValidator>
    where TRequest : class, IDtoRequest<TRequest, TValidator>
    where TValidator : ModelValidatorBase<TRequest>, IValidator<TRequest>
{
    public string CorrelationId { get; set; }
}
