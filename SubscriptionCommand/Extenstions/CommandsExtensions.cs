using SubscriptionCommand.Commands.AcceptInvitation;
using SubscriptionCommand.Commands.CancelInvitation;
using SubscriptionCommand.Commands.RejectInvitation;
using SubscriptionCommand.Commands.SendInvitation;
using SubscriptionCommand.Domain.Enums;
using SubscriptionCommandProto;

namespace SubscriptionCommand.Extenstions
{
    public static class CommandsExtensions
    { 
        public static AcceptInvitationCommand ToCommand(this AcceptInvitationRequest request)
        {
            return new AcceptInvitationCommand(Guid.Parse(request.AccountId) , Guid.Parse(request.MemberId), Guid.Parse(request.SubscriptionId), Guid.Parse(request.UserId));
        }

        public static RejectInvitationCommand ToCommand(this RejectInvitationRequest request)
        {
            return new RejectInvitationCommand(request.AccountId.ToGuid(), request.SubscriptionId.ToGuid(), request.UserId.ToGuid(), request.MemberId.ToGuid());
        }
        public static Guid ToGuid(this string guid) => Guid.Parse(guid);

        public static CancelInvitationCommand ToCommand(this CancelInvitationRequest request)
        {
            return new CancelInvitationCommand(request.AccountId.ToGuid(), request.SubscriptionId.ToGuid(), request.UserId.ToGuid(), request.MemberId.ToGuid());
        }

        public static SendInvitationCommand ToCommand(this SendInvitationRequest request)
        {
            return new SendInvitationCommand(Guid.Parse(request.UserId), Guid.Parse(request.MemberId), Guid.Parse(request.SubscriptionId), (Permissions) request.Permission); ;
        } 

    }
}
