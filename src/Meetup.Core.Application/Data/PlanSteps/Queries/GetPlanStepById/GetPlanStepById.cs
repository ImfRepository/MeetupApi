using Meetup.Core.Application.Common.Interfaces;
using Meetup.Core.Domain.Entities;

namespace Meetup.Core.Application.Data.PlanSteps.Queries.GetPlanStepById;

public record GetPlanStepByIdQuery(int Id) : IRequest<Result<PlanStepEntity>>;

public class GetPlanStepByIdQueryHandler : IRequestHandler<GetPlanStepByIdQuery, Result<PlanStepEntity>>
{
    private readonly IApplicationDbContext _context;

    public GetPlanStepByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PlanStepEntity>> Handle(GetPlanStepByIdQuery request, CancellationToken cancellationToken)
    {
        var step = await _context.PlanSteps
            .Include(e => e.Meetup)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (step == null)
        {
            return Result.Fail(new NotFoundError("Plan step", nameof(step.Id), request.Id.ToString()));
        }

        return step;
    }
}