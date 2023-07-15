namespace ContainerProducts.Api.Core.DataAccess;

public class QueryResponse<TA, TB, TC>
    where TA : QueryOperation
    where TB : QueryOperation
    where TC : QueryOperation
{
    public QueryOperation Operation { get; }

    public QueryResponse(QueryOperation operation)
    {
        Operation = operation;
    }

    public static implicit operator QueryResponse<TA, TB, TC>(TA a) => new(a);

    public static implicit operator QueryResponse<TA, TB, TC>(TB a) => new(a);

    public static implicit operator QueryResponse<TA, TB, TC>(TC c) => new(c);
}