using Microsoft.EntityFrameworkCore;
using SubscriptionCommand.Abstraction;
using SubscriptionCommand.Domain;
using SubscriptionCommand.Events;
using SubscriptionCommand.Infrastructure.MessageBus;

namespace SubscriptionCommand.Infrastructure.Presistance
{
    public class EventStore : IEventStore
    {
        private readonly ApplicationDatabase _db ;
        private readonly AzureMessageBus _publisher;

        public EventStore(ApplicationDatabase db, AzureMessageBus publisher)
        {
            _db = db;
            _publisher = publisher;
        }

        public async Task CommitAsync(UserSubscription userSubscription, CancellationToken cancellationToken)
        {
                var events = userSubscription.GetUncommittedEvents();
             
                var messages = events.Select(x => new Outbox(x));

                await _db.Events.AddRangeAsync(events, cancellationToken);
                await _db.Outbox.AddRangeAsync(messages, cancellationToken);

                await _db.SaveChangesAsync(cancellationToken);

                _publisher.Publish();

        }

        public async Task<List<Event>> GetAllAsync(Guid aggregateId, CancellationToken cancellationToken)
        {
            return await _db.Events.Where(e => e.AggregateId == aggregateId)
                .OrderBy(e => e.Sequence).ToListAsync(cancellationToken: cancellationToken);
        }
        
  
    }
}
