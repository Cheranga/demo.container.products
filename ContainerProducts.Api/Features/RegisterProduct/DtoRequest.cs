using ContainerProducts.Api.Core;

namespace ContainerProducts.Api.Features.RegisterProduct;

public record DtoRequest(string CategoryId, string Name, string Description, decimal UnitPrice);

public class DtoRequestValidator : ModelValidatorBase<DtoRequest>
{
    public DtoRequestValidator()
    {
        throw new NotImplementedException();
    }
}