using static Meetup.Core.Application.Common.Validation.ValidationConstants;

namespace Meetup.Core.Application.Data.Places.Commands.CreatePlace;

internal class CreatePlaceCommandValidator : AbstractValidator<CreatePlaceCommand>
{
    public CreatePlaceCommandValidator()
    {
        RuleFor(e => e.Name)
            .Matches(NoSemicolonRegex)
            .WithMessage(NoSemicolonMsg)
            .Length(2, 50);
    }
}