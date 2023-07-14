using Meetup.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meetup.Infrastructure.Data.EntityConfigs;

public class PlanStepEntityConfig : IEntityTypeConfiguration<PlanStepEntity>
{
    public void Configure(EntityTypeBuilder<PlanStepEntity> builder)
    {
        builder.ToTable("plan_steps");

        builder.Property(p => p.Id)
            .HasColumnName("step_id")
            .UseIdentityAlwaysColumn();

        builder.Property(p => p.Name)
            .HasColumnName("step_name");

        builder.Property(p => p.Time)
            .HasColumnName("step_time");

        builder.Property(p => p.MeetupId)
            .HasColumnName("meetup_id");

        builder
            .HasOne(p => p.Meetup)
            .WithMany(m => m.PlanSteps)
            .HasForeignKey(p => p.MeetupId)
            .HasPrincipalKey(m => m.Id);
    }
}