using System.Runtime.InteropServices.JavaScript;
using SubscriptionCommand.Domain.Enums;

namespace SubscriptionCommand.Domain
{
    public class Invitation
    {
        private Invitation(Guid userId, Guid subscriptionId) {
            UserId = userId;
            SubscriptionId = subscriptionId;
            Status = InvitationStatus.Pending;
            DateTime = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public Guid UserId{ get; set; }
        public Guid SubscriptionId{ get; set; }
        public InvitationStatus Status { get; set; }
        public DateTime DateTime { get; set; }


        public static Invitation Create(Guid userId, Guid subscriptionId)
        {
            return new Invitation(userId, subscriptionId);
        }
    }
}
