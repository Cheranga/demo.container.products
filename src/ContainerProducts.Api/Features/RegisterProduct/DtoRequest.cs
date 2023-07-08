using ContainerProducts.Api.Core;
using ContainerProducts.Api.Extensions;
using FluentValidation;

namespace ContainerProducts.Api.Features.RegisterProduct;

public record DtoRequest(string CategoryId, string ProductId, string Name, decimal UnitPrice)
    : IDtoRequest<DtoRequest, DtoRequest.DtoRequestValidator>
{
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString("N");

    internal DomainRequest ToDomainRequest() =>
        new(CorrelationId, CategoryId, ProductId, Name, UnitPrice);

    public sealed class DtoRequestValidator : ModelValidatorBase<DtoRequest>
    {
        public DtoRequestValidator()
        {
            RuleFor(x => x.CategoryId).NotNullOrEmpty();
            RuleFor(x => x.ProductId).NotNullOrEmpty();
            RuleFor(x => x.Name).NotNullOrEmpty();
            RuleFor(x => x.UnitPrice).GreaterThan(0);
        }
    }
}
