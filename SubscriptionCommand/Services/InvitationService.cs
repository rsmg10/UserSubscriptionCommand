

using Grpc.Core;
using MediatR;
using SubscriptionCommand.Extenstions;
using SubscriptionCommandProto;

namespace SubscriptionCommand.Services;

public class InvitationService : SubscriptionCommandProto.SubscriptionCommand.SubscriptionCommandBase
{
    private readonly ILogger<InvitationService> _logger;
    private readonly IMediator _mediator;

    public InvitationService(ILogger<InvitationService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }


    public override async Task<Response> SendInvitation(SendInvitationRequest request, ServerCallContext context)
    {
        try
        {

            var result = await _mediator.Send(request.ToCommand());
            return result;

        }
        catch (Exception e)
        {

            throw;
        }
    }
    public override async Task<Response> AcceptInvitation(AcceptInvitationRequest request, ServerCallContext context)
    { 
        var result = await _mediator.Send(request.ToCommand());
        return result;
    }
    public override async Task<Response> CancelInvitation(CancelInvitationRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(request.ToCommand());
        return result;
    }
    public override async Task<Response> RejectInvitation(RejectInvitationRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(request.ToCommand());
        return result;
    }
    
}