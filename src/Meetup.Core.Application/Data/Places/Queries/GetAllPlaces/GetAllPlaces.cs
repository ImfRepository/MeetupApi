using Meetup.Core.Application.Common.Interfaces;
using Meetup.Core.Domain.Entities;

namespace Meetup.Core.Application.Data.Places.Queries.GetAllPlaces;

public record GetAllPlacesQuery : IRequest<Result<IEnumerable<PlaceEntity>>>;

public class GetAllPlacesQueryHandler
    : IRequestHandler<GetAllPlacesQuery, Result<IEnumerable<PlaceEntity>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllPlacesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<PlaceEntity>>> Handle(GetAllPlacesQuery request, CancellationToken cancellationToken)
    {
        var places = await _context.Places
            .Include(e => e.Meetups)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (places.Any())
            return places;

        return Result.Fail(new NotFoundError());
    }
}