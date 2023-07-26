using Meetup.Core.Domain.Entities;

namespace Meetup.Core.Application.Data.Organizers.Queries.GetOrganizerById;

public record GetOrganizerByIdQuery(int Id) : IRequest<Result<OrganizerEntity>>;

internal class GetOrganizerByIdQueryHandler : IRequestHandler<GetOrganizerByIdQuery, Result<OrganizerEntity>>
{
    private readonly IApplicationDbContext _context;

    public GetOrganizerByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<OrganizerEntity>> Handle(GetOrganizerByIdQuery request, CancellationToken cancellationToken)
    {
        var organizer = await _context.Organizers
            .Include(e => e.Meetups)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (organizer != null)
            return organizer;

        return Result.Fail(new NotFoundError("Organizer", nameof(organizer.Id), request.Id.ToString()));
    }
}
