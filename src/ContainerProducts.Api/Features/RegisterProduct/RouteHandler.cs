using ContainerProducts.Api.CustomResponses;
using ContainerProducts.Api.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.TypedResults;
using static ContainerProducts.Api.Core.Domain.DomainOperation;
using PC = Microsoft.AspNetCore.Http.HttpResults.Results<
    Microsoft.AspNetCore.Http.HttpResults.ValidationProblem,
    Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult,
    ContainerProducts.Api.CustomResponses.ProductCreated
>;

namespace ContainerProducts.Api.Features.RegisterProduct;

internal static class RouteHandler
{
    public static Func<RegisterProductRequest, RegisterProductRequestHandler, Task<PC>> Handle =>
        async (request, handler) =>
        {
            var token = new CancellationToken();

            var response = await handler.ExecuteAsync(request, token);
            return response.Result switch
            {
                ValidationFailedOperation vf
                    => vf.Failure.ToValidationErrorResponse("Invalid Product Registration"),
                FailedOperation f => ToServerError(f),
                _
                    => new ProductCreated(
                        request.CorrelationId,
                        request.CategoryId,
                        request.ProductId
                    )
            };
        };

    private static ProblemHttpResult ToServerError(FailedOperation failure) =>
        Problem(
            new ProblemDetails
            {
                Type = "Internal Server Error",
                Title = failure.ErrorCode.ToString(),
                Detail = failure.ErrorMessage,
                Status = StatusCodes.Status500InternalServerError
            }
        );
}
