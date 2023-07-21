namespace Infrastructure.Messaging.Azure.Storage.Queues;

public record MessagePublisher : IMessagePublisher
{
    public Task<MPR> PublishAsync(
        string queueName,
        Func<string> messageContent,
        CancellationToken token
    ) => throw new NotImplementedException();

    public Task<MPR> PublishAsync(
        string queueName,
        Func<string> messageContent,
        TimeSpan appearInSeconds,
        TimeSpan expireInSeconds,
        CancellationToken token
    ) => throw new NotImplementedException();
}
