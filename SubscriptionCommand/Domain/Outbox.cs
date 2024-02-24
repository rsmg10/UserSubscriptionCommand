using SubscriptionCommand.Events;

namespace SubscriptionCommand.Domain
{
    public class Outbox
    {
        public Outbox()
        {
            
        }
        public Outbox(Event @event)
        {
            Event = @event;
        }
        public int Id{ get; set; }
        public Event? Event{ get; set; }
    }
}
