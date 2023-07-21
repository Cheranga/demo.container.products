using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Infrastructure.Messaging.Azure.Storage.Queues;

internal record InMemoryMessagePublisher : IMessagePublisher
{
    private ConcurrentQueue<string> Queue { get; } = new();

    public Task<MPR> PublishAsync(
        string queueName,
        Func<string> messageContent,
        CancellationToken token
    )
    {
        Queue.Enqueue(messageContent());
        return Task.FromResult<MPR>(QueueOperation.SuccessOperation.New());
    }

    public Task<MPR> PublishAsync(
        string queueName,
        Func<string> messageContent,
        TimeSpan appearInSeconds,
        TimeSpan expireInSeconds,
        CancellationToken token
    ) => PublishAsync(queueName, messageContent, token);
}
