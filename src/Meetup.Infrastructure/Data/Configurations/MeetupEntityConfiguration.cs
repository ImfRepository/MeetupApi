using Meetup.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meetup.Infrastructure.Data.Configurations;

internal class MeetupEntityConfiguration : IEntityTypeConfiguration<MeetupEntity>
{
    public void Configure(EntityTypeBuilder<MeetupEntity> builder)
    {
        builder.ToTable("meetups");

        builder.Property(p => p.Id)
            .HasColumnName("meetup_id")
            .UseIdentityAlwaysColumn();

        builder.Property(p => p.Name)
            .HasColumnName("meetup_name");

        builder.Property(p => p.Description)
            .HasColumnName("description");

        builder.Property(p => p.OrganizerId)
            .HasColumnName("organizer_id");

        builder.Property(p => p.Speaker)
            .HasColumnName("speaker");

        builder.Property(p => p.Time)
            .HasColumnName("meetup_time");

        builder.Property(p => p.PlaceId)
            .HasColumnName("place_id");
    }
}