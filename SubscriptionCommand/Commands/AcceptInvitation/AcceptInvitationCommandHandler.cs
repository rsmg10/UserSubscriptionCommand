using MediatR;
using SubscriptionCommand.Abstraction;
using SubscriptionCommand.Domain;
using SubscriptionCommand.Exceptions;
using SubscriptionCommand.Extenstions;
using SubscriptionCommandProto;

namespace SubscriptionCommand.Commands.AcceptInvitation
{
    public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Response>
    {
        private readonly IEventStore _eventStore;

        public AcceptInvitationCommandHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task<Response> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == request.MemberId)
                throw new BusinessRuleViolationException("cant send invitation to same user");

            var events = await _eventStore.GetAllAsync(GuidExtensions.CombineGuids(request.SubscriptionId, request.MemberId), cancellationToken);

            if (!events.Any() || events is null)
                throw new BusinessRuleViolationException("invalid Id");
            
            
            var userSubscription = UserSubscription.LoadFromHistory(await _eventStore.GetAllAsync(GuidExtensions.CombineGuids(request.SubscriptionId, request.MemberId), cancellationToken));

            userSubscription.AcceptInvitation(request);
            await _eventStore.CommitAsync(userSubscription, cancellationToken);

            return new Response()
            {
                Id = request.UserId.ToString(),
                Message = "invitation accepted"
            };
        }
    }
}
