using System.Text.Json.Serialization;
using ContainerProducts.Api.Core;
using ContainerProducts.Api.Core.Domain;
using FluentValidation;
using Infrastructure.Messaging.Azure.Storage.Queues;

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
    private readonly IMessagePublisher _messagePublisher;
    private readonly IValidator<RegisterProductRequest> _validator;

    public RegisterProductRequestHandler(
        IValidator<RegisterProductRequest> validator,
        RegisterProductCommandHandler commandHandler,
        IMessagePublisher messagePublisher
    )
    {
        _validator = validator;
        _commandHandler = commandHandler;
        _messagePublisher = messagePublisher;
    }

    public async Task<DR> ExecuteAsync(RegisterProductRequest request, CancellationToken token)
    {
        var validationResult = await _validator.ValidateAsync(request, token);
        if (!validationResult.IsValid)
            return DomainOperation.ValidationFailedOperation.New(validationResult);

        var op = await _messagePublisher.PublishAsync("register-products", () => "", token);
        return op.Operation switch
        {
            QueueOperation.FailedOperation f
                => DomainOperation.FailedOperation.New(f.ErrorCode, f.ErrorMessage, f.Exception),
            _ => DomainOperation.SuccessOperation.New()
        };
    }
}
