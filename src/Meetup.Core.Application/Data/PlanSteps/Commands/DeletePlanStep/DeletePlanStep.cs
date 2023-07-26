namespace Meetup.Core.Application.Data.PlanSteps.Commands.DeletePlanStep;

public record DeletePlanStepCommand(int Id) : IRequest<Result>;

internal class DeletePlanStepCommandHandler : IRequestHandler<DeletePlanStepCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeletePlanStepCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeletePlanStepCommand request, CancellationToken cancellationToken)
    {
        var step = await _context.PlanSteps
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (step == null)
        {
            return Result.Fail(new NotFoundError("Plan step", nameof(step.Id), request.Id.ToString()));
        }

        _context.PlanSteps.Remove(step);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}