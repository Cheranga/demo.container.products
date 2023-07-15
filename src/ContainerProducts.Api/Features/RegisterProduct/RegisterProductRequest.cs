using ContainerProducts.Api.Core;
using ContainerProducts.Api.Core.DataAccess;
using ContainerProducts.Api.Core.Domain;
using FluentValidation;

namespace ContainerProducts.Api.Features.RegisterProduct;

public record RegisterProductRequest(
    string CategoryId,
    string ProductId,
    string Name,
    decimal UnitPrice
) : IRequest<RegisterProductRequest, RegisterProductRequestValidator, RegisterProductRequestHandler>
{
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString("N");
}

internal class RegisterProductRequestHandler : IRequestHandler<RegisterProductRequest>
{
    private readonly RegisterProductCommandHandler _commandHandler;
    private readonly IValidator<RegisterProductRequest> _validator;

    public RegisterProductRequestHandler(
        IValidator<RegisterProductRequest> validator,
        RegisterProductCommandHandler commandHandler
    )
    {
        _validator = validator;
        _commandHandler = commandHandler;
    }

    public async Task<DR> ExecuteAsync(RegisterProductRequest request, CancellationToken token)
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

    private static RegisterProductCommand ToCommand(RegisterProductRequest request) =>
        new(request.CorrelationId, request.CategoryId, request.Name, request.UnitPrice);

    private static DomainOperation.FailedOperation ToFailedOperation(
        CommandOperation.CommandFailedOperation operation
    ) =>
        DomainOperation.FailedOperation.New(
            operation.ErrorCode,
            operation.ErrorMessage,
            operation.Exception
        );
}
