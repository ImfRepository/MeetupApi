namespace Meetup.Core.Application.Data.Meetups.Queries.GetMeetupById;

internal class GetMeetupByIdQueryValidator : AbstractValidator<GetMeetupByIdQuery>
{
    public GetMeetupByIdQueryValidator()
    {
        RuleFor(m => m.Id)
            .GreaterThan(0);
    }
}