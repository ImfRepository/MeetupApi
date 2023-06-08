using Microsoft.EntityFrameworkCore;

namespace Meetup.Infrastructure.Data;

internal class PgContext : DbContext
{
    public PgContext(DbContextOptions<PgContext> opt) : base(opt)
    { }

    public DbSet<MeetupEntity> Meetups { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<PlanStep> PlanSteps { get; set; }
    public DbSet<Organizer> Organizers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MeetupEntity>(e =>
        {
            e.ToTable("events")
                .Property(p => p.Id)
                .UseIdentityAlwaysColumn();

		});


        modelBuilder.Entity<Organizer>(e =>
        {
	        e.Property(p => p.Id)
		        .UseIdentityAlwaysColumn();

			e.ToTable("organizers")
				.HasMany(o => o.Events)
				.WithOne(m => m.Organizer)
				.HasForeignKey(m => m.OrganizerId)
				.HasPrincipalKey(o => o.Id);

		});


        modelBuilder.Entity<Place>(e =>
        {
			e.Property(p => p.Id)
				.UseIdentityAlwaysColumn();

			e.ToTable("places")
				.HasMany(p => p.Events)
				.WithOne(m => m.Place)
				.HasForeignKey(m => m.PlaceId)
				.HasPrincipalKey(p => p.Id);
		});


        modelBuilder.Entity<PlanStep>(e =>
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