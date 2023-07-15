namespace ContainerProducts.Api.Core.Domain;

public class DomainResponse<TA, TB, TC>
    where TA : DomainOperation
    where TB : DomainOperation
    where TC : DomainOperation
{
    public DomainOperation Result { get; }

    private DomainResponse(DomainOperation result)
    {
        Result = result;
    }

    public static implicit operator DomainResponse<TA, TB, TC>(TA a) => new(a);

    public static implicit operator DomainResponse<TA, TB, TC>(TB b) => new(b);

    public static implicit operator DomainResponse<TA, TB, TC>(TC c) => new(c);
}