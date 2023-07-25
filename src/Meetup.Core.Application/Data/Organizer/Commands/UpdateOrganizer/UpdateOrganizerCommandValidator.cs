using static Meetup.Core.Application.Common.Validation.ValidationConstants;

namespace Meetup.Core.Application.Data.Organizer.Commands.UpdateOrganizer;

public class UpdateOrganizerCommandValidator : AbstractValidator<UpdateOrganizerCommand>
{
    public UpdateOrganizerCommandValidator()
    {
        RuleFor(e => e.Id)
            .GreaterThan(0);

        RuleFor(e => e.Name)
            .Matches(NoSemicolonRegex)
            .WithMessage(NoSemicolonMsg)
            .Length(2, 50);
    }
}