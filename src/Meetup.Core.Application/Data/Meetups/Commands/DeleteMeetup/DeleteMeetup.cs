namespace Meetup.Core.Application.Data.Meetups.Commands.DeleteMeetup;

public record DeleteMeetupCommand(int Id) : IRequest<Result>;

internal class DeleteMeetupCommandHandler :
    IRequestHandler<DeleteMeetupCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteMeetupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteMeetupCommand request, CancellationToken cancellationToken)
    {
        var meetup = await _context.Meetups
            .Include(e => e.PlanSteps)
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (meetup == null)
        {
            return Result.Fail(new NotFoundError("Meetup", nameof(meetup.Id), request.Id.ToString()));
        }

        _context.Meetups.Remove(meetup);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}