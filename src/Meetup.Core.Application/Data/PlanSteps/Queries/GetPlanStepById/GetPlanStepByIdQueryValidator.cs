namespace Meetup.Core.Application.Data.PlanSteps.Queries.GetPlanStepById;

internal class GetPlanStepByIdQueryValidator : AbstractValidator<GetPlanStepByIdQuery>
{
    public GetPlanStepByIdQueryValidator()
    {
        RuleFor(e => e.Id)
            .GreaterThan(0);
    }
}