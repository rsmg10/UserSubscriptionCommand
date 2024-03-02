﻿using SubscriptionCommand.Commands.AcceptInvitation;
using SubscriptionCommand.Commands.CancelInvitation;
using SubscriptionCommand.Commands.ChangePermission;
using SubscriptionCommand.Commands.JoinMember;
using SubscriptionCommand.Commands.LeaveSubscription;
using SubscriptionCommand.Commands.RejectInvitation;
using SubscriptionCommand.Commands.RemoveMember;
using SubscriptionCommand.Commands.SendInvitation;
using SubscriptionCommand.Events;

namespace SubscriptionCommand.Extensions
{
    public static class EventsExtensions
    {

        public static InvitationSent ToEvent(this SendInvitationCommand command, int sequence)
        {
            return new InvitationSent(GuidExtensions.CombineGuids(command.SubscriptionId, command.MemberId),
                new InvitationSentData(command.UserId,
                    command.SubscriptionId, 
                    command.Permission),
                DateTime.UtcNow,
                sequence,
                command.UserId.ToString(),
                Version: 1);
        }


        public static InvitationRejected ToEvent(this RejectInvitationCommand command, int sequence)
        {
            return new InvitationRejected(GuidExtensions.CombineGuids(command.SubscriptionId, command.MemberId),
                null,
                DateTime.UtcNow,
                sequence,
                command.UserId.ToString(),
                Version: 1);
        }
        public static InvitationCancelled ToEvent(this CancelInvitationCommand commnad, int sequence)
        {
            return new InvitationCancelled(GuidExtensions.CombineGuids(commnad.SubscriptionId, commnad.MemberId),
                null,
                DateTime.UtcNow,
                sequence,
                commnad.UserId.ToString(),
                Version: 1);
        }
        public static InvitationAccepted ToEvent(this AcceptInvitationCommand command, int sequence)
        {
            return new InvitationAccepted(GuidExtensions.CombineGuids(command.SubscriptionId, command.MemberId),
                null,
                DateTime.UtcNow,
                sequence,
                command.UserId.ToString(),
                Version: 1);
        }
        public static PermissionChanged ToEvent(this ChangePermissionCommand command, int sequence)
        {
            return new PermissionChanged(GuidExtensions.CombineGuids(command.SubscriptionId, command.MemberId),
                new PermissionChangedData(command.AccountId, command.UserId, command.MemberId, command.SubscriptionId, command.Permission),
                DateTime.UtcNow,
                sequence,
                command.UserId.ToString(),
                Version: 1);
        }

        public static MemberJoined ToEvent(this JoinMemberCommand command, int sequence)
        {
            return new MemberJoined(GuidExtensions.CombineGuids(command.SubscriptionId, command.MemberId),
                new MemberJoinedData(command.AccountId, command.UserId, command.MemberId, command.SubscriptionId, command.Permission),
                DateTime.UtcNow,
                sequence,
                command.UserId.ToString(),
                Version: 1);


        } public static MemberLeft ToEvent(this LeaveSubscriptionCommand command, int sequence)
        {
            return new MemberLeft(GuidExtensions.CombineGuids(command.SubscriptionId, command.MemberId),
                new MemberLeftData(command.AccountId, command.MemberId, command.SubscriptionId),
                DateTime.UtcNow,
                sequence,
                command.MemberId.ToString(),
                Version: 1);
        }
        public static MemberRemoved ToEvent(this RemoveMemberCommand request, int sequence)
        {
            return new MemberRemoved(GuidExtensions.CombineGuids(request.SubscriptionId, request.command),
                new MemberRemovedData(request.UserId, request.SubscriptionId, request.command),
                DateTime.UtcNow,
                sequence,
                request.command.ToString(),
                Version: 1);
        }
    }
}