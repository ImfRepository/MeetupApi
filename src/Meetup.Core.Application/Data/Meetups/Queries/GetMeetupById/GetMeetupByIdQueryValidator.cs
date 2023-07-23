namespace Meetup.Core.Application.Data.Meetups.Queries.GetMeetupById;

public class GetMeetupByIdQueryValidator : AbstractValidator<GetMeetupByIdQuery>
{
    public GetMeetupByIdQueryValidator()
    {
        RuleFor(m => m.Id)
            .GreaterThan(0);
    }
}