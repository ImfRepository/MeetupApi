namespace Meetup.Core.Application.Data.Places.Queries.GetPlaceById;

internal class GetPlaceByIdQueryValidator : AbstractValidator<GetPlaceByIdQuery>
{
    public GetPlaceByIdQueryValidator()
    {
        RuleFor(e => e.Id)
            .GreaterThan(0);
    }
}