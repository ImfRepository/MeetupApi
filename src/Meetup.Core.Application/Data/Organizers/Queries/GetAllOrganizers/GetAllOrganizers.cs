using Meetup.Core.Domain.Entities;

namespace Meetup.Core.Application.Data.Organizers.Queries.GetAllOrganizers;

public record GetAllOrganizersQuery() : IRequest<Result<IEnumerable<OrganizerEntity>>>;

internal class GetAllOrganizersQueryHandler
    : IRequestHandler<GetAllOrganizersQuery, Result<IEnumerable<OrganizerEntity>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllOrganizersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<OrganizerEntity>>> Handle(GetAllOrganizersQuery request, CancellationToken cancellationToken)
    {
        var organizers = await _context.Organizers
            .Include(e => e.Meetups)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (organizers.Any())
            return organizers;

        return Result.Fail(new NotFoundError());
    }
}
