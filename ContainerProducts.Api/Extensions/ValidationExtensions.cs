using FluentValidation;

namespace ContainerProducts.Api.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> NotNullOrEmpty<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.NotNull().WithMessage("cannot be null")
            .NotEmpty().WithMessage("cannot be empty");
}