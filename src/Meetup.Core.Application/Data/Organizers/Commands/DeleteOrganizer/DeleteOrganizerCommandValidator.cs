namespace Meetup.Core.Application.Data.Organizers.Commands.DeleteOrganizer;

internal class DeleteOrganizerCommandValidator : AbstractValidator<DeleteOrganizerCommand>
{
    public DeleteOrganizerCommandValidator()
    {
        RuleFor(e => e.Id)
            .GreaterThan(0);
    }
}