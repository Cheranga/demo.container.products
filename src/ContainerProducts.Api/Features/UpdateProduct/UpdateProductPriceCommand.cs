namespace ContainerProducts.Api.Features.RegisterProduct;

internal sealed record UpdateProductPriceCommand(
    string CorrelationId,
    string CategoryId,
    string Name,
    string Description,
    decimal UnitPrice
);

internal record UpdateProductPriceCommandHandler
{
    
}