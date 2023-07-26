using Meetup.Core.Domain.Entities;

namespace Meetup.Core.Application.Data.Meetups.Queries.GetMeetupById;

public record GetMeetupByIdQuery(int Id) : IRequest<Result<MeetupEntity>>;

internal class GetMeetupByIdQueryHandler : IRequestHandler<GetMeetupByIdQuery, Result<MeetupEntity>>
{
	private readonly IApplicationDbContext _context;

	public GetMeetupByIdQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<MeetupEntity>> Handle(GetMeetupByIdQuery request, CancellationToken cancellationToken)
	{
		var meetup = await _context.Meetups
			.AsNoTracking()
			.Include(e => e.Place)
			.Include(e => e.Organizer)
			.Include(e => e.PlanSteps)
			.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

		if (meetup == null)
			return Result.Fail(new NotFoundError("Meetup", nameof(meetup.Id), request.Id.ToString()));

		return meetup;
	}
}