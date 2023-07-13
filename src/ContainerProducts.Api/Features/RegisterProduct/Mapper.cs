using ContainerProducts.Api.Core.DataAccess;
using ContainerProducts.Api.Core.Domain;

namespace ContainerProducts.Api.Features.RegisterProduct;

internal static class Mapper
{
    internal static RegisterProductCommand ToCommand(this RegisterProductRequest request) =>
        new(request.CorrelationId, request.CategoryId, request.Name, request.UnitPrice);

    internal static DomainOperation.FailedOperation ToFailedOperation(
        this CommandOperation.CommandFailedOperation operation
    ) =>
        DomainOperation.FailedOperation.New(
            operation.ErrorCode,
            operation.ErrorMessage,
            operation.Exception
        );
}
