namespace Meetup.Core.Application.Data.Organizer.Commands.DeleteOrganizer;

public class DeleteOrganizerCommandValidator : AbstractValidator<DeleteOrganizerCommand>
{
    public DeleteOrganizerCommandValidator()
    {
        RuleFor(e => e.Id)
            .GreaterThan(0);
    }
}