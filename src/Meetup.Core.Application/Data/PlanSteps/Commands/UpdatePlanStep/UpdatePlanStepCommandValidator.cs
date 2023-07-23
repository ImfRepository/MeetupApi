﻿using static Meetup.Core.Application.Data.ValidationConstants;

namespace Meetup.Core.Application.Data.PlanSteps.Commands.UpdatePlanStep;

public class UpdatePlanStepCommandValidator : AbstractValidator<UpdatePlanStepCommand>
{
    public UpdatePlanStepCommandValidator()
    {
        RuleFor(e => e.Id)
            .GreaterThan(0);

        RuleFor(e => e.Time)
            .GreaterThan(MinDateTime)
            .LessThan(MaxDateTime);

        RuleFor(e => e.Name)
            .Matches(NoSemicolonRegex)
            .WithMessage(NoSemicolonMsg)
            .Length(2, 50);
    }
}