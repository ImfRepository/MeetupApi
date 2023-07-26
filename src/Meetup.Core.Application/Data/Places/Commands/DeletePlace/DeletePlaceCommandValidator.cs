namespace Meetup.Core.Application.Data.Places.Commands.DeletePlace;

internal class DeletePlaceCommandValidator : AbstractValidator<DeletePlaceCommand>
{
    public DeletePlaceCommandValidator()
    {
        RuleFor(e => e.Id)
            .GreaterThan(0);
    }
}