using Meetup.Core.Application.Common.Interfaces;
using Meetup.Core.Domain.Entities;

namespace Meetup.Core.Application.Data.Meetups.Queries.GetAllMeetups;

public record GetAllMeetupsQuery : IRequest<Result<IEnumerable<MeetupEntity>>>;

public class GetAllMeetupsQueryHandler
    : IRequestHandler<GetAllMeetupsQuery, Result<IEnumerable<MeetupEntity>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllMeetupsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<MeetupEntity>>> Handle(GetAllMeetupsQuery request, CancellationToken cancellationToken)
    {
        var meetups = await _context.Meetups
            .AsNoTracking()
            .Include(e => e.Organizer)
            .Include(e => e.Place)
            .Include(e => e.PlanSteps)
            .ToListAsync(cancellationToken);

        if (meetups.Any())
            return meetups;

        return Result.Fail(new NotFoundError());
    }
}