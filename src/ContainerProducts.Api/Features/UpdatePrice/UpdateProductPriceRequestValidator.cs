using ContainerProducts.Api.Core;
using ContainerProducts.Api.Extensions;
using FluentValidation;

namespace ContainerProducts.Api.Features.UpdatePrice;

public class UpdateProductPriceRequestValidator : ModelValidatorBase<UpdateProductPriceRequest>
{
    public UpdateProductPriceRequestValidator()
    {
        RuleFor(x => x.CategoryId).NotNullOrEmpty();
        RuleFor(x => x.ProductId).NotNullOrEmpty();
        RuleFor(x => x.UnitPrice).GreaterThan(0);
    }
}
