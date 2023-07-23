namespace Meetup.Core.Application.Data.PlanSteps.Queries.GetAllPlanSteps;

public class GetAllPlanStepsQueryValidator : AbstractValidator<GetAllPlanStepsQuery>
{
    public GetAllPlanStepsQueryValidator()
    {
        RuleFor(e => e.MeetupId)
            .GreaterThan(0);
    }
}