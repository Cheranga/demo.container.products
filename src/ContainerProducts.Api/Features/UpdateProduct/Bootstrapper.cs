using FluentValidation;

namespace ContainerProducts.Api.Features.UpdateProduct;

public static class Bootstrapper
{
    public static IServiceCollection UseUpdateProduct(this IServiceCollection services) =>
        services
            .AddScoped<DomainRequestHandler>()
            .AddValidatorsFromAssemblyContaining<DomainRequest>();
}
