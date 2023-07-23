using Meetup.Core.Application.Common.Interfaces;

namespace Meetup.Core.Application.Data.Places.Commands.DeletePlace;

public record DeletePlaceCommand(int Id) : IRequest<Result>;

public class DeletePlaceCommandHandler : IRequestHandler<DeletePlaceCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeletePlaceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeletePlaceCommand request, CancellationToken cancellationToken)
    {
        var place = await _context.Places
            .Include(e => e.Meetups)
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (place == null)
        {
            return Result.Fail(new NotFoundError("Place", nameof(place.Id), request.Id.ToString()));
        }

        _context.Places.Remove(place);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}