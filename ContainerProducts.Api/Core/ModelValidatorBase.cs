using FluentValidation;
using FluentValidation.Results;

namespace ContainerProducts.Api.Core;

public class ModelValidatorBase<T> : AbstractValidator<T> where T : class
{
    public ModelValidatorBase() => ClassLevelCascadeMode = CascadeMode.Stop;

    protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
    {
        if (context.InstanceToValidate == null)
        {
            result.Errors.Add(new ValidationFailure("", "instance is null"));
            return false;
        }

        return base.PreValidate(context, result);
    }
}
