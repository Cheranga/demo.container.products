using ContainerProducts.Api.CustomResponses;
using ContainerProducts.Api.Extensions;
using FluentValidation;
using Infrastructure.Messaging.Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.TypedResults;
using PU = Microsoft.AspNetCore.Http.HttpResults.Results<
    Microsoft.AspNetCore.Http.HttpResults.ValidationProblem,
    Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult,
    ContainerProducts.Api.CustomResponses.ProductUpdatedResponse
>;

namespace ContainerProducts.Api.Features.UpdatePrice;

internal static class RouteHandler
{
    public static Func<
        UpdateProductPriceRequest,
        IValidator<UpdateProductPriceRequest>,
        IMessagePublisher,
        Task<PU>
    > Handle =>
        async (request, validator, publisher) =>
        {
            var token = new CancellationToken();
            var validationResult = await validator.ValidateAsync(request, token);
            if (!validationResult.IsValid)
                return validationResult.ToValidationErrorResponse("Invalid Update Price Request");

            var operation = await publisher.PublishAsync(
                "update-product",
                request.GetMessageContent(),
                token
            );
            return operation.Operation switch
            {
                QueueOperation.FailedOperation f
                    => Problem(
                        new ProblemDetails
                        {
                            Type = "Internal Server Error",
                            Title = f.ErrorCode.ToString(),
                            Detail = f.ErrorMessage,
                            Status = StatusCodes.Status500InternalServerError
                        }
                    ),
                _
                    => new ProductUpdatedResponse(
                        request.CorrelationId,
                        request.CategoryId,
                        request.ProductId
                    )
            };
        };
}
