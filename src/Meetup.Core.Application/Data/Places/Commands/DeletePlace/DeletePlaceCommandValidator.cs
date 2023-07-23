namespace Meetup.Core.Application.Data.Places.Commands.DeletePlace;

public class DeletePlaceCommandValidator : AbstractValidator<DeletePlaceCommand>
{
    public DeletePlaceCommandValidator()
    {
        RuleFor(e => e.Id)
            .GreaterThan(0);
    }
}