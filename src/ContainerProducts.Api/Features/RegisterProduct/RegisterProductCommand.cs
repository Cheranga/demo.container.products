using ContainerProducts.Api.Core.DataAccess;
using static ContainerProducts.Api.Core.DataAccess.CommandOperation;

namespace ContainerProducts.Api.Features.RegisterProduct;

internal sealed record RegisterProductCommand(
    string CorrelationId,
    string CategoryId,
    string Name,
    decimal UnitPrice
);

internal record RegisterProductCommandHandler
{
    public Task<CommandResponse<CommandFailedOperation, CommandSuccessOperation>> ExecuteAsync(
        RegisterProductCommand command,
        CancellationToken token
    )
    {
        return Task.FromResult<CommandResponse<CommandFailedOperation, CommandSuccessOperation>>(
            Success()
        );
    }
}
