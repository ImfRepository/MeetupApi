using static Meetup.Core.Application.Common.Validation.ValidationConstants;

namespace Meetup.Core.Application.Data.PlanSteps.Commands.CreatePlanStep;

public class CreatePlanStepCommandValidator : AbstractValidator<CreatePlanStepCommand>
{
    public CreatePlanStepCommandValidator()
    {
        RuleFor(e => e.MeetupId)
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