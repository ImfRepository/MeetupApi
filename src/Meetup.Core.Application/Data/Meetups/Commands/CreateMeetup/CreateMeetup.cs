using Meetup.Core.Application.Common.Interfaces;
using Meetup.Core.Domain.Entities;

namespace Meetup.Core.Application.Data.Meetups.Commands.CreateMeetup;

public record CreateMeetupCommand : IRequest<Result<int>>
{
    public string Name { get; init; } = null!;

    public string Description { get; init; } = null!;

    public int OrganizerId { get; init; }

    public string Speaker { get; init; } = null!;

    public DateTime Time { get; init; }

    public int PlaceId { get; init; }
}

public class CreateMeetupCommandHandler : IRequestHandler<CreateMeetupCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public CreateMeetupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreateMeetupCommand request, CancellationToken cancellationToken)
    {
        var meetup = new MeetupEntity()
        {
            Name = request.Name,
            Description = request.Description,
            OrganizerId = request.OrganizerId,
            Speaker = request.Speaker,
            Time = request.Time,
            PlaceId = request.PlaceId,
        };

        await _context.Meetups.AddAsync(meetup, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return meetup.Id;
    }
}