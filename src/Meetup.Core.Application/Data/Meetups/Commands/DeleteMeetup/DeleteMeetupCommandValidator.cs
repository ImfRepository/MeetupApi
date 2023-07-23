namespace Meetup.Core.Application.Data.Meetups.Commands.DeleteMeetup;

public class DeleteMeetupCommandValidator : AbstractValidator<DeleteMeetupCommand>
{
    public DeleteMeetupCommandValidator()
    {
        RuleFor(m => m.Id)
            .GreaterThan(0);
    }
}