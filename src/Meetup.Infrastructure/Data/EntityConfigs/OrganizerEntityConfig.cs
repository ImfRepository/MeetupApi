using Meetup.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meetup.Infrastructure.Data.EntityConfigs;

public class OrganizerEntityConfig : IEntityTypeConfiguration<OrganizerEntity>
{
    public void Configure(EntityTypeBuilder<OrganizerEntity> builder)
    {
        builder.ToTable("organizers");

        builder.Property(p => p.Id)
            .HasColumnName("organizer_id")
            .UseIdentityAlwaysColumn();

        builder.Property(p => p.Name)
            .HasColumnName("organizer_name");

        builder
            .HasMany(o => o.Meetups)
            .WithOne(m => m.Organizer)
            .HasForeignKey(m => m.OrganizerId)
            .HasPrincipalKey(o => o.Id);
    }
}