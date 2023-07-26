namespace Meetup.Core.Application.Data.Organizers.Commands.UpdateOrganizer;

public record UpdateOrganizerCommand : IRequest<Result>
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;
}

internal class UpdateOrganizerCommandHandler : IRequestHandler<UpdateOrganizerCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateOrganizerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateOrganizerCommand request, CancellationToken cancellationToken)
    {
        var organizer = await _context.Organizers
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (organizer == null)
        {
            return Result.Fail(new NotFoundError("Organizer", nameof(organizer.Id), request.Id.ToString()));
        }

        organizer.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}