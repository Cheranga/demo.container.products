using ContainerProducts.Api.Core;

namespace ContainerProducts.Api.Features.RegisterProduct;

internal sealed record DomainRequest(
    string CorrelationId,
    string CategoryId,
    string Name,
    string Description,
    decimal UnitPrice
);

internal sealed class DomainRequestValidator : ModelValidatorBase<DomainRequest>
{
    public DomainRequestValidator() => throw new NotImplementedException();
}