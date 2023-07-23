using static Meetup.Core.Application.Data.ValidationConstants;

namespace Meetup.Core.Application.Data.Organizer.Commands.CreateOrganizer;

public class CreateOrganizerCommandValidator : AbstractValidator<CreateOrganizerCommand>
{
    public CreateOrganizerCommandValidator()
    {
        RuleFor(e => e.Name)
            .Matches(NoSemicolonRegex)
            .WithMessage(NoSemicolonMsg)
            .Length(2, 50);
    }
}