namespace ContainerProducts.Api.Core.DataAccess;

public class CommandResponse<TA, TB>
    where TA : CommandOperation
    where TB : CommandOperation
{
    public CommandOperation Result { get; }

    private CommandResponse(CommandOperation operation)
    {
        Result = operation;
    }

    public static implicit operator CommandResponse<TA, TB>(TA a) => new(a);

    public static implicit operator CommandResponse<TA, TB>(TB b) => new(b);
}
