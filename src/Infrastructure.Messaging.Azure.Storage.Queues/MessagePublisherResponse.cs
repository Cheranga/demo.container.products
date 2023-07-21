namespace Infrastructure.Messaging.Azure.Storage.Queues;

public class MessagePublisherResponse<TA, TB>
    where TA : QueueOperation
    where TB : QueueOperation
{
    private MessagePublisherResponse(QueueOperation operation) => Operation = operation;

    public QueueOperation Operation { get; }

    public static implicit operator MessagePublisherResponse<TA, TB>(TA a) => new(a);

    public static implicit operator MessagePublisherResponse<TA, TB>(TB b) => new(b);
}
