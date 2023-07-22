using ContainerProducts.Api.CustomResponses;
using ContainerProducts.Api.Extensions;
using FluentValidation;
using Infrastructure.Messaging.Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.TypedResults;
using PC = Microsoft.AspNetCore.Http.HttpResults.Results<
    Microsoft.AspNetCore.Http.HttpResults.ValidationProblem,
    Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult,
    ContainerProducts.Api.CustomResponses.ProductCreatedResponse
>;

namespace ContainerProducts.Api.Features.RegisterProduct;

internal static class RouteHandler
{
    public static Func<
        RegisterProductRequest,
        IValidator<RegisterProductRequest>,
        IMessagePublisher,
        Task<PC>
    > Handle =>
        async (request, validator, publisher) =>
        {
            var token = new CancellationToken();
            var validationResult = await validator.ValidateAsync(request, token);
            if (!validationResult.IsValid)
                return validationResult.ToValidationErrorResponse("Invalid Product Registration");

            var operation = await publisher.PublishAsync(
                "register-products",
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
                    => new ProductCreatedResponse(
                        request.CorrelationId,
                        request.CategoryId,
                        request.ProductId
                    )
            };
        };
}
