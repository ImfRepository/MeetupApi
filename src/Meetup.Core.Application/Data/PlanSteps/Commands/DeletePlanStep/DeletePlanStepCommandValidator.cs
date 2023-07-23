namespace Meetup.Core.Application.Data.PlanSteps.Commands.DeletePlanStep;

public class DeletePlanStepCommandValidator : AbstractValidator<DeletePlanStepCommand>
{
    public DeletePlanStepCommandValidator()
    {
        RuleFor(e => e.Id)
            .GreaterThan(0);
    }
}