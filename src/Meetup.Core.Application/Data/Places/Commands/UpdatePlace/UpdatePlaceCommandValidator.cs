using static Meetup.Core.Application.Data.ValidationConstants;

namespace Meetup.Core.Application.Data.Places.Commands.UpdatePlace;

public class UpdatePlaceCommandValidator : AbstractValidator<UpdatePlaceCommand>
{
    public UpdatePlaceCommandValidator()
    {
        RuleFor(e => e.Id)
            .GreaterThan(0);

        RuleFor(e => e.Name)
            .Matches(NoSemicolonRegex)
            .WithMessage(NoSemicolonMsg)
            .Length(2, 50);
    }
}