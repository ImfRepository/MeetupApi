using Meetup.Core.Application.Common.Interfaces;

namespace Meetup.Core.Application.Data.PlanSteps.Commands.UpdatePlanStep;

public record UpdatePlanStepCommand : IRequest<Result>
{
    public int Id { get; init; }

    public DateTime Time { get; init; }

    public string Name { get; init; } = string.Empty;
}

public class UpdatePlanStepCommandHandler : IRequestHandler<UpdatePlanStepCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdatePlanStepCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdatePlanStepCommand request, CancellationToken cancellationToken)
    {
        var step = await _context.PlanSteps
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (step == null)
        {
            return Result.Fail(new NotFoundError("Plan step", nameof(step.Id), request.Id.ToString()));
        }

        step.Name = request.Name;

        step.Time = request.Time;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}