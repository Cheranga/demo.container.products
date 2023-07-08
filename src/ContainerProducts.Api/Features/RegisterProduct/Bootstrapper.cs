using FluentValidation;

namespace ContainerProducts.Api.Features.RegisterProduct;

public static class Bootstrapper
{
    public static IServiceCollection UseRegisterProduct(this IServiceCollection services) =>
        services
            .AddScoped<DomainRequestHandler>()
            .AddValidatorsFromAssemblyContaining<DomainRequest>();
}
