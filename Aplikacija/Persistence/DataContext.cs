using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Court> Courts { get; set; }
        public DbSet<Activity> Activities { get; set; } // pristup tabeli activity
        public DbSet<ActivityParticipant> ActivityParticipants { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<TimeSlot> TimeSlot { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<TeamParticipant> TeamParticipants { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<PrivateActivity> PrivateActivities { get; set; }

        public DbSet<Challenge> Challenges { get; set; }





        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();

            builder.Entity<ActivityParticipant>(x => x.HasKey(ap => new { ap.AppUserId, ap.ActivityId }));

            builder.Entity<ActivityParticipant>()
                .HasOne(u => u.AppUser)
                .WithMany(a => a.Activities)
                .HasForeignKey(aa => aa.AppUserId);

            builder.Entity<ActivityParticipant>()
                .HasOne(a => a.Activity)
                .WithMany(u => u.Participants)
                .HasForeignKey(aa => aa.ActivityId);

            

            builder.Entity<PrivateActivity>()
                .HasOne(a => a.Team)
                .WithMany(c => c.PrivateActivities)
                .HasForeignKey(a => a.TeamId);

            

            




            builder.Entity<TeamParticipant>(x => x.HasKey(tp => new { tp.AppUserId, tp.TeamId }));

            builder.Entity<TeamParticipant>()
                .HasOne(u => u.AppUser)
                .WithMany(t => t.Teams)
                .HasForeignKey(tp => tp.AppUserId);

            builder.Entity<TeamParticipant>()
                .HasOne(t => t.Team)
                .WithMany(u => u.Participants)
                .HasForeignKey(tp => tp.TeamId);

            builder.Entity<Activity>()
                .HasOne(a => a.Court)
                .WithMany(c => c.Activities)
                .HasForeignKey(a => a.CourtId);

            

                

            builder.Entity<AppUser>()
                .HasMany(u => u.GivenRatings)
                .WithOne(r => r.RatedByUser)
                .HasForeignKey(r => r.RatedByUserId)
                .IsRequired();

            builder.Entity<AppUser>()
                .HasMany(u => u.ReceivedRatings)
                .WithOne(r => r.RatedUser)
                .HasForeignKey(r => r.RatedUserId)
                .IsRequired();


            builder.Entity<Message>()
                .HasOne(m => m.Team)
                .WithMany(a => a.Messages)
                .OnDelete(DeleteBehavior.Cascade); // kada se obrise team, obrisi i sve poruke vezane za taj team
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableDetailedErrors();
        }
    }
}