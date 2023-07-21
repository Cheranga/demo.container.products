namespace ContainerProducts.Api.Features.RegisterProduct;

public static class Bootstrapper
{
    public static IServiceCollection UseRegisterProduct(this IServiceCollection services) =>
        services
            .AddScoped<RegisterProductRequestHandler>()
            .AddScoped<RegisterProductCommandHandler>();
}
