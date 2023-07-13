using ContainerProducts.Api.Core;
using ContainerProducts.Api.Core.DataAccess;
using ContainerProducts.Api.Core.Domain;
using ContainerProducts.Api.Extensions;
using FluentValidation;

namespace ContainerProducts.Api.Features.RegisterProduct;

public record RegisterProductRequest(
    string CategoryId,
    string ProductId,
    string Name,
    decimal UnitPrice
)
    : IRequest<
        RegisterProductRequest,
        RegisterProductRequest.Validator,
        RegisterProductRequest.Handler
    >
{
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString("N");

    public sealed class Validator : ModelValidatorBase<RegisterProductRequest>
    {
        public Validator()
        {
            RuleFor(x => x.CategoryId).NotNullOrEmpty();
            RuleFor(x => x.ProductId).NotNullOrEmpty();
            RuleFor(x => x.Name).NotNullOrEmpty();
            RuleFor(x => x.UnitPrice).GreaterThan(0);
        }
    }

    internal sealed class Handler : IRequestHandler<RegisterProductRequest>
    {
        private readonly IValidator<RegisterProductRequest> _validator;
        private readonly RegisterProductCommandHandler _commandHandler;

        public Handler(IValidator<RegisterProductRequest> validator, RegisterProductCommandHandler commandHandler)
        {
            _validator = validator;
            _commandHandler = commandHandler;
        }
        public async Task<
            DomainResponse<
                DomainOperation.ValidationFailedOperation,
                DomainOperation.FailedOperation,
                DomainOperation.SuccessOperation
            >
        > ExecuteAsync(RegisterProductRequest request, CancellationToken token)
        {
            var validationResult = await _validator.ValidateAsync(request, token);
            if (!validationResult.IsValid)
                return DomainOperation.ValidationFailedOperation.New(validationResult);

            var operation = await _commandHandler.ExecuteAsync(request.ToCommand(), token);
            return operation.Result switch
            {
                CommandOperation.CommandFailedOperation f => f.ToFailedOperation(),
                _ => DomainOperation.SuccessOperation.New()
            };
        }
    }
}
