using ContainerProducts.Api.Core;
using ContainerProducts.Api.Extensions;
using FluentValidation;

namespace ContainerProducts.Api.Features.RegisterProduct;

internal sealed record DomainRequest(
    string CorrelationId,
    string CategoryId,
    string ProductId,
    string Name,
    decimal UnitPrice
)
{
    public sealed class DomainRequestValidator : ModelValidatorBase<DomainRequest>
    {
        public DomainRequestValidator()
        {
            RuleFor(x => x.CategoryId).NotNullOrEmpty();
            RuleFor(x => x.ProductId).NotNullOrEmpty();
            RuleFor(x => x.Name).NotNullOrEmpty();
            RuleFor(x => x.UnitPrice).GreaterThan(0);
        }
    }    
}

internal sealed record DomainRequestHandler
{
    // TODO: specify what it can return
    public Task RegisterProductAsync(DomainRequest request)
    {
        return Task.CompletedTask;
    }
}