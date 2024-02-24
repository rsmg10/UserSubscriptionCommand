using SubscriptionCommand.Commands.AcceptInvitation;
using SubscriptionCommand.Commands.CancelInvitation;
using SubscriptionCommand.Commands.RejectInvitation;
using SubscriptionCommand.Commands.SendInvitation;
using SubscriptionCommand.Events;
using SubscriptionCommand.Extenstions;

namespace SubscriptionCommand.Extensions
{
    public static class EventsExtensions
    { 
       
        public static InvitationSent ToEvent(this SendInvitationCommand request, int sequence)
        {
            return new InvitationSent(GuidExtensions.CombineGuids(request.SubscriptionId, request.MemberId),
                new InvitationSentData(request.UserId,
                request.SubscriptionId),
                DateTime.UtcNow,
                sequence,
                request.UserId.ToString(),
                Version: 1);
        }
 
       
        public static InvitationRejected ToEvent(this RejectInvitationCommand request, int sequence)
        {
            return new InvitationRejected(GuidExtensions.CombineGuids(request.SubscriptionId, request.MemberId),
                null,
                DateTime.UtcNow,
                sequence,
                request.UserId.ToString(),
                Version: 1);
        }
        
        public static InvitationCancelled ToEvent(this CancelInvitationCommand request, int sequence)
        {
            return new InvitationCancelled(GuidExtensions.CombineGuids(request.SubscriptionId, request.MemberId),
                null,
                DateTime.UtcNow,
                sequence,
                request.UserId.ToString(),
                Version: 1);
        }
        public static InvitationAccepted ToEvent(this AcceptInvitationCommand request, int sequence)
        {
            return new InvitationAccepted(GuidExtensions.CombineGuids(request.SubscriptionId, request.MemberId),
                null,
                DateTime.UtcNow,
                sequence,
                request.UserId.ToString(),
                Version: 1);
        }

        
    }
}
