namespace Infrastructure.Messaging.Azure.Storage.Queues;

public interface IMessagePublisher
{
    Task<MPR> PublishAsync(string queueName, Func<string> messageContent, CancellationToken token);

    Task<MPR> PublishAsync(
        string queueName,
        Func<string> messageContent,
        TimeSpan appearInSeconds,
        TimeSpan expireInSeconds,
        CancellationToken token
    );
}
