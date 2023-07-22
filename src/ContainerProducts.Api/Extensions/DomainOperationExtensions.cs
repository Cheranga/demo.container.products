using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.TypedResults;
using static ContainerProducts.Api.Core.Domain.DomainOperation;

namespace ContainerProducts.Api.Extensions;

public static class DomainOperationExtensions
{
    public static ProblemHttpResult ToServerError(
        this FailedOperation failure,
        string type = "Internal Server Error"
    ) =>
        Problem(
            new ProblemDetails
            {
                Type = type,
                Title = failure.ErrorCode.ToString(),
                Detail = failure.ErrorMessage,
                Status = StatusCodes.Status500InternalServerError
            }
        );
}
