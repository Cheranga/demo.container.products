using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging.Azure.Storage.Queues;

public static class Bootstrapper
{
    public static IServiceCollection UseInMemoryMessaging(this IServiceCollection services) =>
        services.AddSingleton<IMessagePublisher, InMemoryMessagePublisher>();
    
    // TODO: implement the Azure storage queue registration
}
