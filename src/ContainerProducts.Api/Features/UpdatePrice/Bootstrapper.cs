namespace ContainerProducts.Api.Features.UpdatePrice;

public static class Bootstrapper
{
    public static IServiceCollection UseUpdateProduct(this IServiceCollection services) =>
        services
            .AddScoped<UpdateProductPriceRequestHandler>()
            .AddScoped<UpdateProductPriceCommandHandler>();
}
