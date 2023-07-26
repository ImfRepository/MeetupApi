namespace Meetup.Core.Application.Data.Organizers.Commands.DeleteOrganizer;

public record DeleteOrganizerCommand(int Id) : IRequest<Result>;

internal class DeleteOrganizerCommandHandler : IRequestHandler<DeleteOrganizerCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteOrganizerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteOrganizerCommand request, CancellationToken cancellationToken)
    {
        var organizer = await _context.Organizers
            .Include(e => e.Meetups)
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (organizer == null)
        {
            return Result.Fail(new NotFoundError("Organizer", nameof(organizer.Id), request.Id.ToString()));
        }

        _context.Organizers.Remove(organizer);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}