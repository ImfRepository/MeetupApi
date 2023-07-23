using Meetup.Core.Application.Common.Interfaces;

namespace Meetup.Core.Application.Data.Places.Commands.UpdatePlace;

public record UpdatePlaceCommand : IRequest<Result>
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;
}

public class UpdatePlaceCommandHandler : IRequestHandler<UpdatePlaceCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdatePlaceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdatePlaceCommand request, CancellationToken cancellationToken)
    {
        var place = await _context.Places
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (place == null)
        {
            return Result.Fail(new NotFoundError("Place", nameof(place.Id), request.Id.ToString()));
        }

        place.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}