namespace SubscriptionCommand.Events
{
    public record Event(
        int Id, 
        Guid AggregateId, 
        DateTime DateTime,
        int Sequence, 
        string UserId,
        int Version
        )
    {
        public string Type => GetType().Name;
    }

    public record Event<T>(
    Guid AggregateId,
    T Data,
    DateTime DateTime,
    int Sequence,
    string UserId, 
    int Version
    ) : Event( Id: default, AggregateId: AggregateId, DateTime: DateTime, Sequence: Sequence, UserId: UserId, Version: Version);
 
}
