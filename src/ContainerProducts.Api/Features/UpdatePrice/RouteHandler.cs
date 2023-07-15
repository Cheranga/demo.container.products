using ContainerProducts.Api.CustomResponses;
using ContainerProducts.Api.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ContainerProducts.Api.Features.UpdatePrice;

internal static class RouteHandler
{
    public static Func<
        UpdateProductPriceRequest,
        IValidator<UpdateProductPriceRequest>,
        UpdateProductPriceRequestHandler,
        Task<Results<ValidationProblem, ProductUpdated>>
    > Handle =>
        async (request, validator, handler) =>
        {
            var token = new CancellationToken();

            var validationResult = await validator.ValidateAsync(request, token);
            if (!validationResult.IsValid)
                return validationResult.ToValidationErrorResponse("Update Product Price");

            await handler.ExecuteAsync(request, token);

            return new ProductUpdated(request.CorrelationId, request.CategoryId, request.ProductId);
        };
}
