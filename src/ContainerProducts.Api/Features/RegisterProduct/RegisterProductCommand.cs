namespace ContainerProducts.Api.Features.RegisterProduct;

internal sealed record RegisterProductCommand(
    string CorrelationId,
    string CategoryId,
    string Name,
    string Description,
    decimal UnitPrice
);

internal record RegisterProductCommandHandler
{
    
}