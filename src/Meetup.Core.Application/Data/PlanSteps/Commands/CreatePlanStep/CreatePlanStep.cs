using Meetup.Core.Domain.Entities;

namespace Meetup.Core.Application.Data.PlanSteps.Commands.CreatePlanStep;

public record CreatePlanStepCommand : IRequest<Result<int>>
{
    public int MeetupId { get; init; }

    public DateTime Time { get; init; }

    public string Name { get; init; } = string.Empty;
}

internal class CreatePlanStepCommandHandler : IRequestHandler<CreatePlanStepCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public CreatePlanStepCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreatePlanStepCommand request, CancellationToken cancellationToken)
    {
        var planStep = new PlanStepEntity()
        {
            MeetupId = request.MeetupId,
            Time = request.Time,
            Name = request.Name
        };

        await _context.PlanSteps.AddAsync(planStep, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return planStep.Id;
    }
}