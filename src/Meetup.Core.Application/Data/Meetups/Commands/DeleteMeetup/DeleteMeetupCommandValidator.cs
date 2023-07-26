namespace Meetup.Core.Application.Data.Meetups.Commands.DeleteMeetup;

internal class DeleteMeetupCommandValidator : AbstractValidator<DeleteMeetupCommand>
{
    public DeleteMeetupCommandValidator()
    {
        RuleFor(m => m.Id)
            .GreaterThan(0);
    }
}