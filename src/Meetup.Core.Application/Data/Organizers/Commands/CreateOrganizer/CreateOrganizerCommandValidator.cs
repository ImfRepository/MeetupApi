using static Meetup.Core.Application.Common.Validation.ValidationConstants;

namespace Meetup.Core.Application.Data.Organizers.Commands.CreateOrganizer;

internal class CreateOrganizerCommandValidator : AbstractValidator<CreateOrganizerCommand>
{
    public CreateOrganizerCommandValidator()
    {
        RuleFor(e => e.Name)
            .Matches(NoSemicolonRegex)
            .WithMessage(NoSemicolonMsg)
            .Length(2, 50);
    }
}