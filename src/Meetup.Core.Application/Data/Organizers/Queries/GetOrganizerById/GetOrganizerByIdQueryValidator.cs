namespace Meetup.Core.Application.Data.Organizers.Queries.GetOrganizerById;

internal class GetOrganizerByIdQueryValidator : AbstractValidator<GetOrganizerByIdQuery>
{
    public GetOrganizerByIdQueryValidator()
    {
        RuleFor(e => e.Id)
            .GreaterThan(0);
    }
}