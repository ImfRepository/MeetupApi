using static Meetup.Core.Application.Data.ValidationConstants;

namespace Meetup.Core.Application.Data.Places.Commands.CreatePlace;

public class CreatePlaceCommandValidator : AbstractValidator<CreatePlaceCommand>
{
    public CreatePlaceCommandValidator()
    {
        RuleFor(e => e.Name)
            .Matches(NoSemicolonRegex)
            .WithMessage(NoSemicolonMsg)
            .Length(2, 50);
    }
}