namespace ContainerProducts.Api.Features.UpdatePrice;

public record ProductPriceUpdatedEvent(string CategoryId, string ProductId, decimal Price);
