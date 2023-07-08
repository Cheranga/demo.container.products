using ContainerProducts.Api.Core;
using ContainerProducts.Api.Extensions;
using FluentValidation;

namespace ContainerProducts.Api.Features.UpdateProduct;

public record DtoRequest(string CategoryId, string ProductId, decimal UnitPrice)
    : IDtoRequest<DtoRequest, DtoRequest.DtoRequestValidator>
{
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString("N");

    internal DomainRequest ToDomainRequest() =>
        new(CorrelationId, CategoryId, ProductId, UnitPrice);

    public sealed class DtoRequestValidator : ModelValidatorBase<DtoRequest>
    {
        public DtoRequestValidator()
        {
            RuleFor(x => x.CategoryId).NotNullOrEmpty();
            RuleFor(x => x.ProductId).NotNullOrEmpty();
            RuleFor(x => x.UnitPrice).GreaterThan(0);
        }
    }
}
