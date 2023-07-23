namespace Meetup.Core.Application.Data.Places.Queries.GetPlaceById;

public class GetPlaceByIdQueryValidator : AbstractValidator<GetPlaceByIdQuery>
{
    public GetPlaceByIdQueryValidator()
    {
        RuleFor(e => e.Id)
            .GreaterThan(0);
    }
}