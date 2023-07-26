using static Meetup.Core.Application.Common.Validation.ValidationConstants;

namespace Meetup.Core.Application.Data.Organizers.Commands.UpdateOrganizer;

internal class UpdateOrganizerCommandValidator : AbstractValidator<UpdateOrganizerCommand>
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