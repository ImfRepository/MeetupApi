using Meetup.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Meetup.Infrastructure.Data;

internal class PgContext : DbContext
{
    public PgContext(DbContextOptions<PgContext> opt) : base(opt)
    { }

    public DbSet<MeetupDto> Meetups { get; set; }
    public DbSet<PlaceDto> Places { get; set; }
    public DbSet<PlanStepDto> PlanSteps { get; set; }
    public DbSet<OrganizerDto> Organizers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MeetupDto>(e =>
        {
            e.ToTable("meetups")
                .Property(p => p.Id)
                .UseIdentityAlwaysColumn();

		});

        modelBuilder.Entity<OrganizerDto>(e =>
        {
	        e.Property(p => p.Id)
		        .UseIdentityAlwaysColumn();

			e.ToTable("organizers")
				.HasMany(o => o.Meetups)
				.WithOne(m => m.OrganizerDto)
				.HasForeignKey(m => m.OrganizerId)
				.HasPrincipalKey(o => o.Id);

		});

        modelBuilder.Entity<PlaceDto>(e =>
        {
			e.Property(p => p.Id)
				.UseIdentityAlwaysColumn();

			e.ToTable("places")
				.HasMany(p => p.Meetups)
				.WithOne(m => m.PlaceDto)
				.HasForeignKey(m => m.PlaceId)
				.HasPrincipalKey(p => p.Id);
		});

        modelBuilder.Entity<PlanStepDto>(e =>
        {
	        e.Property(p => p.Id)
		        .UseIdentityAlwaysColumn();

			e.ToTable("plan_steps")
				.HasOne(p => p.Meetup)
				.WithMany(m => m.PlanSteps)
				.HasForeignKey(p => p.MeetupId)
				.HasPrincipalKey(m => m.Id);
		});
    }
}