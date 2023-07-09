using ContainerProducts.Api.Core;
using ContainerProducts.Api.Core.Domain;
using ContainerProducts.Api.Extensions;
using FluentValidation;
using FluentValidation.Results;
using static ContainerProducts.Api.Core.DataAccess.CommandOperation;
using static ContainerProducts.Api.Core.Domain.DomainOperation;

namespace ContainerProducts.Api.Features.RegisterProduct;

public sealed record DomainRequest(
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
    private readonly RegisterProductCommandHandler _commandHandler;
    private readonly IValidator<DomainRequest> _domainValidator;
    private readonly IValidator<DtoRequest> _dtoValidator;
    private readonly ILogger<DomainRequestHandler> _logger;

    public DomainRequestHandler(
        IValidator<DtoRequest> dtoValidator,
        IValidator<DomainRequest> domainValidator,
        RegisterProductCommandHandler commandHandler,
        ILogger<DomainRequestHandler> logger
    )
    {
        _dtoValidator = dtoValidator;
        _domainValidator = domainValidator;
        _commandHandler = commandHandler;
        _logger = logger;
    }
    // TODO: specify what it can return
    public async Task<
        DomainResponse<ValidationFailedOperation, FailedOperation, SuccessOperation>
    > RegisterProductAsync(DtoRequest request, CancellationToken token)
    {
        var validationResult = await _dtoValidator.ValidateAsync(request, token);
        if (!validationResult.IsValid)
            return ValidationFailedOperation.New(validationResult);

        var domainRequest = request.ToDomainRequest();
        validationResult = await _domainValidator.ValidateAsync(domainRequest, token);
        if (!validationResult.IsValid)
            return ValidationFailedOperation.New(validationResult);

        var command = domainRequest.ToCommand();
        var operation = await _commandHandler.ExecuteAsync(command, token);
        return operation.Result switch
        {
            CommandFailedOperation f
                => FailedOperation.New(f.ErrorCode, f.ErrorMessage, f.Exception),
            _ => SuccessOperation.New()
        };
    }
}
