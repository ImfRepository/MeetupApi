using Meetup.Core.Application.Common.Interfaces;
using Meetup.Core.Domain.Entities;

namespace Meetup.Core.Application.Data.PlanSteps.Queries.GetAllPlanSteps;

public record GetAllPlanStepsQuery(int MeetupId) : IRequest<Result<IEnumerable<PlanStepEntity>>>;

public class GetAllPlanStepsQueryHandler
    : IRequestHandler<GetAllPlanStepsQuery, Result<IEnumerable<PlanStepEntity>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllPlanStepsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<PlanStepEntity>>> Handle(GetAllPlanStepsQuery request, CancellationToken cancellationToken)
    {
        var steps = await _context.PlanSteps
            .Include(e => e.Meetup)
            .AsNoTracking()
            .Where(e => e.MeetupId == request.MeetupId)
            .ToListAsync(cancellationToken);

        if (steps.Any())
            return steps;

        return Result.Fail(new NotFoundError());
    }
}