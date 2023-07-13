using ContainerProducts.Api.Core;
using ContainerProducts.Api.Core.Domain;
using ContainerProducts.Api.Extensions;
using FluentValidation;

namespace ContainerProducts.Api.Features.UpdateProduct;

public record UpdateProductRequest(string CategoryId, string ProductId, decimal UnitPrice)
    : IRequest<
        UpdateProductRequest,
        UpdateProductRequest.Validator,
        UpdateProductRequest.Handler
    >
{
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString("N");

    public sealed class Validator : ModelValidatorBase<UpdateProductRequest>
    {
        public Validator()
        {
            RuleFor(x => x.CategoryId).NotNullOrEmpty();
            RuleFor(x => x.ProductId).NotNullOrEmpty();
            RuleFor(x => x.UnitPrice).GreaterThan(0);
        }
    }

    internal sealed record Handler : IRequestHandler<UpdateProductRequest>
    {
        public Task<
            DomainResponse<
                DomainOperation.ValidationFailedOperation,
                DomainOperation.FailedOperation,
                DomainOperation.SuccessOperation
            >
        > ExecuteAsync(UpdateProductRequest request, CancellationToken token)
        {
            var response = DomainOperation.SuccessOperation.New();
            return Task.FromResult<
                DomainResponse<
                    DomainOperation.ValidationFailedOperation,
                    DomainOperation.FailedOperation,
                    DomainOperation.SuccessOperation
                >
            >(response);
        }
    }
}
