using Meetup.Core.Domain.Entities;

namespace Meetup.Core.Application.Data.Places.Commands.CreatePlace;

public record CreatePlaceCommand(string Name) : IRequest<Result<int>>;

internal class CreatePlaceCommandHandler : IRequestHandler<CreatePlaceCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public CreatePlaceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreatePlaceCommand request, CancellationToken token)
    {
        var place = new PlaceEntity()
        {
            Name = request.Name
        };

        await _context.Places.AddAsync(place, token);
        await _context.SaveChangesAsync(token);

        return place.Id;
    }
}