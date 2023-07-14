using Meetup.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meetup.Infrastructure.Data.EntityConfigs;

public class PlaceEntityConfig : IEntityTypeConfiguration<PlaceEntity>
{
    public void Configure(EntityTypeBuilder<PlaceEntity> builder)
    {
        builder.ToTable("places");

        builder.Property(p => p.Id)
            .HasColumnName("place_id")
            .UseIdentityAlwaysColumn();

        builder.Property(p => p.Name)
            .HasColumnName("place_name");

        builder
            .HasMany(p => p.Meetups)
            .WithOne(m => m.Place)
            .HasForeignKey(m => m.PlaceId)
            .HasPrincipalKey(p => p.Id);
    }
}