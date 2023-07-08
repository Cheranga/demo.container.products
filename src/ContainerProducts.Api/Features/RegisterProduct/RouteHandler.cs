using ContainerProducts.Api.CustomResponses;
using ContainerProducts.Api.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using static ContainerProducts.Api.CustomResponses.Factory;

namespace ContainerProducts.Api.Features.RegisterProduct;

internal static class RouteHandler
{
    public static Func<
        string,
        DtoRequest,
        IValidator<DtoRequest>,
        DomainRequestHandler,
        Task<Results<ValidationProblem, ProductCreated>>
    > Handle =>
        async (correlationId, request, validator, handler) =>
        {
            var token = new CancellationToken();
            request.CorrelationId = correlationId;

            var validationResult = await validator.ValidateAsync(request, token);
            if (!validationResult.IsValid)
                return validationResult.ToValidationErrorResponse("Register Product");

            await handler.RegisterProductAsync(request.ToDomainRequest());

            return ProductCreated(correlationId, request.CategoryId, request.ProductId);
        };
}
