namespace Meetup.Core.Application.Data.PlanSteps.Queries.GetAllPlanSteps;

internal class GetAllPlanStepsQueryValidator : AbstractValidator<GetAllPlanStepsQuery>
{
    public GetAllPlanStepsQueryValidator()
    {
        RuleFor(e => e.MeetupId)
            .GreaterThan(0);
    }
}