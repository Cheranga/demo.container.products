namespace ContainerProducts.Api.Features.RegisterProduct;

internal static class Mapper
{
    internal static DomainRequest ToDomainRequest(this DtoRequest request) =>
        new(
            request.CorrelationId,
            request.CategoryId,
            request.ProductId,
            request.Name,
            request.UnitPrice
        );

    internal static RegisterProductCommand ToCommand(this DomainRequest request) =>
        new(request.CorrelationId, request.CategoryId, request.Name, request.UnitPrice);
}
