using ContainerProducts.Api.Core;
using ContainerProducts.Api.Core.DataAccess;
using ContainerProducts.Api.Core.Domain;
using FluentValidation;

namespace ContainerProducts.Api.Features.UpdatePrice;

public record UpdateProductPriceRequest(string CategoryId, string ProductId, decimal UnitPrice)
    : IRequest<
        UpdateProductPriceRequest,
        UpdateProductPriceRequestValidator,
        UpdateProductPriceRequestHandler
    >
{
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString("N");
}

internal sealed record UpdateProductPriceRequestHandler : IRequestHandler<UpdateProductPriceRequest>
{
    private readonly IValidator<UpdateProductPriceRequest> _validator;
    private readonly UpdateProductPriceCommandHandler _commandHandler;

    public UpdateProductPriceRequestHandler(
        IValidator<UpdateProductPriceRequest> validator,
        UpdateProductPriceCommandHandler commandHandler
    )
    {
        _validator = validator;
        _commandHandler = commandHandler;
    }

    public async Task<DR> ExecuteAsync(UpdateProductPriceRequest request, CancellationToken token)
    {
        var validationResult = await _validator.ValidateAsync(request, token);
        if (!validationResult.IsValid)
            return DomainOperation.ValidationFailedOperation.New(validationResult);

        var operation = await _commandHandler.ExecuteAsync(ToCommand(request), token);
        return operation.Result switch
        {
            CommandOperation.CommandFailedOperation f => ToFailedOperation(f),
            _ => DomainOperation.SuccessOperation.New()
        };
    }

    private static UpdateProductPriceCommand ToCommand(UpdateProductPriceRequest request) =>
        new(request.CorrelationId, request.CategoryId, request.ProductId, request.UnitPrice);

    private static DomainOperation.FailedOperation ToFailedOperation(
        CommandOperation.CommandFailedOperation operation
    ) =>
        DomainOperation.FailedOperation.New(
            operation.ErrorCode,
            operation.ErrorMessage,
            operation.Exception
        );
}
