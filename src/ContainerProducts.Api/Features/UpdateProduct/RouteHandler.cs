using ContainerProducts.Api.CustomResponses;
using ContainerProducts.Api.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using static ContainerProducts.Api.CustomResponses.Factory;

namespace ContainerProducts.Api.Features.UpdateProduct;

internal static class RouteHandler
{
    public static Func<
        string,
        UpdateProductRequest,
        IValidator<UpdateProductRequest>,
        UpdateProductRequest.Handler,
        Task<Results<ValidationProblem, ProductUpdated>>
    > Handle =>
        async (correlationId, request, validator, handler) =>
        {
            var token = new CancellationToken();
            request.CorrelationId = correlationId;

            var validationResult = await validator.ValidateAsync(request, token);
            if (!validationResult.IsValid)
                return validationResult.ToValidationErrorResponse("Update Product Price");

            await handler.ExecuteAsync(request, token);

            return ProductUpdated(correlationId, request.CategoryId, request.ProductId);
        };
}
