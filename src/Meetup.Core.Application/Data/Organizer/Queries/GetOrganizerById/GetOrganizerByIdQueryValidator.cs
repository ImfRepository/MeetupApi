namespace Meetup.Core.Application.Data.Organizer.Queries.GetOrganizerById;

public class GetOrganizerByIdQueryValidator : AbstractValidator<GetOrganizerByIdQuery>
{
    public GetOrganizerByIdQueryValidator()
    {
        RuleFor(e => e.Id)
            .GreaterThan(0);
    }
}