using FluentValidation;
using FluentValidation.Results;

namespace ContainerProducts.Api.Core;

public abstract class DtoRequestHandlerBase<TDto, TDtoValidator>
    where TDto : class, IDtoRequest<TDto, TDtoValidator>
    where TDtoValidator : ModelValidatorBase<TDto>
{
    private readonly ILogger<DtoRequestHandlerBase<TDto, TDtoValidator>> _logger;
    private readonly IValidator<TDto> _validator;

    protected DtoRequestHandlerBase(
        IValidator<TDto> validator,
        ILogger<DtoRequestHandlerBase<TDto, TDtoValidator>> logger
    )
    {
        _validator = validator;
        _logger = logger;
    }

    protected virtual Task<ValidationResult> ValidateAsync(TDto request, CancellationToken token)
    {
        _logger.LogInformation(
            "Validating request for {@Request} in {CorrelationId}",
            request,
            request.CorrelationId
        );
        return _validator.ValidateAsync(request, token);
    }
}

public interface IDtoRequest<TRequest, TValidator>
    where TRequest : class, IDtoRequest<TRequest, TValidator>
    where TValidator : ModelValidatorBase<TRequest>, IValidator<TRequest>
{
    public string CorrelationId { get; set; }

    public Task<ValidationResult> ValidateRequestAsync(TValidator validator, TRequest request) =>
        validator.ValidateAsync(request);
}
