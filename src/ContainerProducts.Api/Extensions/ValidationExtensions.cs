using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.AspNetCore.Http.TypedResults;

namespace ContainerProducts.Api.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> NotNullOrEmpty<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.NotNull().WithMessage("cannot be null")
            .NotEmpty().WithMessage("cannot be empty");

    public static ValidationProblem ToValidationErrorResponse(this ValidationResult validationResult, string title) =>
        ValidationProblem(validationResult.ToDictionary(), type: "Invalid Request", title: title);

}