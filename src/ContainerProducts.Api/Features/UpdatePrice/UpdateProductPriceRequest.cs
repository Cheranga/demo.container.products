using System.Text.Json;
using ContainerProducts.Api.Core;
using ContainerProducts.Api.Core.Domain;
using FluentValidation;
using Infrastructure.Messaging.Azure.Storage.Queues;

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
    private readonly IMessagePublisher _messagePublisher;
    private readonly IValidator<UpdateProductPriceRequest> _validator;

    public UpdateProductPriceRequestHandler(
        IValidator<UpdateProductPriceRequest> validator,
        IMessagePublisher messagePublisher
    )
    {
        _validator = validator;
        _messagePublisher = messagePublisher;
    }

    public async Task<DR> ExecuteAsync(
        UpdateProductPriceRequest request,
        CancellationToken token = new()
    )
    {
        var validationResult = await _validator.ValidateAsync(request, token);
        if (!validationResult.IsValid)
            return DomainOperation.ValidationFailedOperation.New(validationResult);

        var op = await _messagePublisher.PublishAsync(
            "update-products",
            GetMessageContent(request),
            token
        );
        return op.Operation switch
        {
            QueueOperation.FailedOperation f
                => DomainOperation.FailedOperation.New(f.ErrorCode, f.ErrorMessage, f.Exception),
            _ => DomainOperation.SuccessOperation.New()
        };
    }

    private static Func<string> GetMessageContent(UpdateProductPriceRequest request) =>
        () =>
            JsonSerializer.Serialize(
                request,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );
}
