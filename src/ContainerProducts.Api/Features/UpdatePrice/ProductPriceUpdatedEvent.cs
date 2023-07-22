namespace ContainerProducts.Api.Features.UpdatePrice;

public record ProductPriceUpdatedEvent(
    string CorrelationId,
    string CategoryId,
    string ProductId,
    decimal Price
);
