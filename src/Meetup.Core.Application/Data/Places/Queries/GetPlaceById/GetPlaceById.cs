using Meetup.Core.Application.Common.Interfaces;
using Meetup.Core.Domain.Entities;

namespace Meetup.Core.Application.Data.Places.Queries.GetPlaceById;

public record GetPlaceByIdQuery(int Id) : IRequest<Result<PlaceEntity>>;

public class GetPlaceByIdQueryHandler : IRequestHandler<GetPlaceByIdQuery, Result<PlaceEntity>>
{
    private readonly IApplicationDbContext _context;

    public GetPlaceByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PlaceEntity>> Handle(GetPlaceByIdQuery request, CancellationToken cancellationToken)
    {
        var place = await _context.Places
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (place != null)
            return place;

        return Result.Fail(new NotFoundError("Place", nameof(place.Id), request.Id.ToString()));
    }
}