namespace ContainerProducts.Api.Features.RegisterProduct;

public record ProductRegisteredEvent(string CorrelationId, string CategoryId, string ProductId);
