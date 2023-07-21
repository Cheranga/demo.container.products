using ContainerProducts.Api.Core.DataAccess;
using static ContainerProducts.Api.Core.DataAccess.CommandOperation;

namespace ContainerProducts.Api.Features.UpdatePrice;

internal sealed record UpdateProductPriceCommand(
    string CorrelationId,
    string CategoryId,
    string ProductId,
    decimal UnitPrice
);

internal record UpdateProductPriceCommandHandler
{
    public Task<CommandResponse<CommandFailedOperation, CommandSuccessOperation>> ExecuteAsync(
        UpdateProductPriceCommand command,
        CancellationToken token
    )
    {
        return Task.FromResult<CommandResponse<CommandFailedOperation, CommandSuccessOperation>>(
            Success()
        );
    }
}
