using Microsoft.AspNetCore.Mvc.ModelBinding;
using SubscriptionCommand.Commands.SendInvitation;
using SubscriptionCommand.Domain.Enums;
using SubscriptionCommand.Events;
using SubscriptionCommand.Extenstions;
using System.Security.Principal;
using SubscriptionCommand.Abstraction;
using SubscriptionCommand.Commands.AcceptInvitation;
using SubscriptionCommand.Commands.CancelInvitation;
using SubscriptionCommand.Commands.RejectInvitation;
using SubscriptionCommand.Exceptions;
using SubscriptionCommand.Extensions;

namespace SubscriptionCommand.Domain
{
    public class UserSubscription : Aggregate<UserSubscription>, IAggregate
    {
        public UserSubscription() { }
        public Guid UserId { get; set; }
        public SubscriptionType Type { get; set; }
        private List<Invitation> Invitations { get; set; } = new List<Invitation>();
        private bool IsJoined { get; set; }


        public void RejectInvitation(RejectInvitationCommand command)
        {
 
            if (IsJoined)
                throw new AlreadySentException("This user already joined");
            
            if (Invitations.Last().Status is not InvitationStatus.Pending)
                throw new AlreadySentException("You do not have a pending invitation with this user");
            
            if (Type is SubscriptionType.Personal)
                throw new BusinessRuleViolationException("this subscription's type is invalid");
            

            ApplyNewChange(command.ToEvent(Sequence + 1));
        }
        
        public void AcceptInvitation(AcceptInvitationCommand command)
        {
            if (IsJoined)
                throw new AlreadySentException("This user already joined");
            
            if (Invitations.Last().Status is not InvitationStatus.Pending)
                throw new AlreadySentException("You do not have a pending invitation with this user");
            
            if (Type is SubscriptionType.Personal)
                throw new BusinessRuleViolationException("this subscription's type is invalid");
            

            ApplyNewChange(command.ToEvent(Sequence + 1));
            
        }
        
        
        public void CancelInvitation(CancelInvitationCommand command)
        {
            if (command.UserId != UserId)
                throw new BusinessRuleViolationException("you are not allowed to cancel this invitation");
            if (IsJoined)
            {
                throw new AlreadySentException("This user already joined");
            }
            if (Invitations.Last().Status is not InvitationStatus.Pending)
            {
                throw new AlreadySentException("You do not have a pending invitation with this user");
            }
            if (Type is SubscriptionType.Personal)
            {
                throw new BusinessRuleViolationException("this subscription's type is invalid");
            }

            ApplyNewChange(command.ToEvent(Sequence + 1));

        }
        public void SendInvitation(SendInvitationCommand command)
        {
            if (IsJoined)
            {
                throw new AlreadySentException("This user already joined");
            }
            if (Invitations.Any() &&  Invitations.Last().Status is not InvitationStatus.Rejected)
            {
                throw new AlreadySentException("You Already have an invitation with this user");
            }
            if (Type is SubscriptionType.Personal)
            {
                throw new BusinessRuleViolationException("this subscription's type is invalid");
            }
            
            ApplyNewChange(command.ToEvent(Sequence + 1));
              
        }
        protected override void Mutate(Event @event)
        {
            switch (@event)
            {
                case InvitationSent invitationSent:
                    Mutate(invitationSent);
                    break;
                case InvitationCancelled invitationCancelled:
                    Mutate(invitationCancelled);
                    break;
                case InvitationAccepted invitationAccepted:
                    Mutate(invitationAccepted);
                    break;
                case InvitationRejected invitationRejected:
                    Mutate(invitationRejected);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void Mutate(InvitationCancelled @event)
        {
            var invitation = Invitations.MaxBy(x=> x.Id) ?? throw new ArgumentNullException();
            invitation.Status = InvitationStatus.Cancelled;
        }

        private void Mutate(InvitationRejected @event)
        {
            var invitation = Invitations.MaxBy(x=> x.Id) ?? throw new ArgumentNullException();
            invitation.Status = InvitationStatus.Rejected;
            IsJoined = false;
        }

        private void Mutate(InvitationAccepted @event)
        {
            var invitation = Invitations.MaxBy(x=> x.Id) ?? throw new ArgumentNullException();
            invitation.Status = InvitationStatus.Accepted;
            IsJoined = true;
        }

        private void Mutate(InvitationSent @event)
        {
            Invitations.Add(Invitation.Create(@event.Data.UserId, @event.Data.SubscriptionId));
        }
    }
}
