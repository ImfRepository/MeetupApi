using Meetup.Core.Application.Common.Interfaces;

namespace Meetup.Core.Application.Data.Meetups.Commands.UpdateMeetup;

public record UpdateMeetupCommand : IRequest<Result>
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public string Description { get; init; } = null!;

    public int OrganizerId { get; init; }

    public string Speaker { get; init; } = null!;

    public DateTime Time { get; init; }

    public int PlaceId { get; init; }
}

public class UpdateMeetupCommandHandler : IRequestHandler<UpdateMeetupCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateMeetupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateMeetupCommand request, CancellationToken cancellationToken)
    {
        var meetup = await _context.Meetups
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (meetup == null)
        {
            return Result.Fail(new NotFoundError("Meetup", nameof(meetup.Id), request.Id.ToString()));
        }

        meetup.Name = request.Name;

        meetup.Description = request.Description;

        meetup.OrganizerId = request.OrganizerId;

        meetup.Speaker = request.Speaker;

        meetup.PlaceId = request.PlaceId;

        meetup.Time = request.Time;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}