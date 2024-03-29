﻿namespace Meetup.Core.Domain.Entities;

public class PlaceEntity
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual IEnumerable<MeetupEntity> Meetups { get; set; } = null!;
}