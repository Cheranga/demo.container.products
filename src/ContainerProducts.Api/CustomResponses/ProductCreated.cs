using System.Net;

namespace ContainerProducts.Api.CustomResponses;

internal class ProductCreated : IResult
{
    private readonly string _categoryId;
    private readonly string _correlationId;
    private readonly string _productId;

    public ProductCreated(string correlationId, string categoryId, string productId)
    {
        _correlationId = correlationId;
        _categoryId = categoryId;
        _productId = productId;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.Headers.Location = $"api/products/{_categoryId}/{_productId}";
        httpContext.Response.Headers.Add("CorrelationId", _correlationId);
        httpContext.Response.StatusCode = (int)HttpStatusCode.Created;
        return Task.CompletedTask;
    }
}
