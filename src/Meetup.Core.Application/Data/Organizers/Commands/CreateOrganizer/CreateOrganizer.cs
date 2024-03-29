﻿using Meetup.Core.Domain.Entities;

namespace Meetup.Core.Application.Data.Organizers.Commands.CreateOrganizer;

public record CreateOrganizerCommand(string Name) : IRequest<Result<int>>;

internal class CreateOrganizerCommandHandler : IRequestHandler<CreateOrganizerCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public CreateOrganizerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreateOrganizerCommand request, CancellationToken cancellationToken)
    {
        var organizer = new OrganizerEntity
        {
            Name = request.Name
        };

        await _context.Organizers.AddAsync(organizer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return organizer.Id;
    }
}