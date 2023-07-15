using ContainerProducts.Api.Core;
using ContainerProducts.Api.Extensions;
using FluentValidation;

namespace ContainerProducts.Api.Features.RegisterProduct;

public sealed class RegisterProductRequestValidator : ModelValidatorBase<RegisterProductRequest>
{
    public RegisterProductRequestValidator()
    {
        RuleFor(x => x.CategoryId).NotNullOrEmpty();
        RuleFor(x => x.ProductId).NotNullOrEmpty();
        RuleFor(x => x.Name).NotNullOrEmpty();
        RuleFor(x => x.UnitPrice).GreaterThan(0);
    }
}
