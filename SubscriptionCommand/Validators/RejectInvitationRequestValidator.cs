﻿using FluentValidation;
using SubscriptionCommandProto;

namespace SubscriptionCommand.Validators;

public class RejectInvitationRequestValidator : AbstractValidator<RejectInvitationRequest>
{
    public RejectInvitationRequestValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().NotNull();
        RuleFor(x => x.MemberId).NotEmpty().NotNull(); 
        RuleFor(x => x.SubscriptionId).NotEmpty().NotNull();
        RuleFor(x => x.UserId).NotEmpty().NotNull();
    }
    
}