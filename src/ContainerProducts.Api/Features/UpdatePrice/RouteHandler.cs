using ContainerProducts.Api.CustomResponses;
using ContainerProducts.Api.Extensions;
using FluentValidation;
using PU = Microsoft.AspNetCore.Http.HttpResults.Results<
    Microsoft.AspNetCore.Http.HttpResults.ValidationProblem,
    Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult,
    ContainerProducts.Api.CustomResponses.ProductUpdated
>;

namespace ContainerProducts.Api.Features.UpdatePrice;

internal static class RouteHandler
{
    public static Func<
        UpdateProductPriceRequest,
        IValidator<UpdateProductPriceRequest>,
        UpdateProductPriceRequestHandler,
        Task<PU>
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
