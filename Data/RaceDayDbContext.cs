using Microsoft.EntityFrameworkCore;
using RaceDay.API.Models;

namespace RaceDay.API.Data
{
    public class RaceDayDbContext : DbContext
    {
        public RaceDayDbContext(DbContextOptions<RaceDayDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // UNIQUE CONSTRAINTS
            // ==========================================

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Role_Name)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Category_Name)
                .IsUnique();

            modelBuilder.Entity<EventCategory>()
                .HasIndex(ec => new { ec.Event_ID, ec.Category_ID })
                .IsUnique();

            modelBuilder.Entity<Enrolment>()
                .HasIndex(e => new { e.Event_Category_ID, e.Participant_ID })
                .IsUnique();

            modelBuilder.Entity<Result>()
                .HasIndex(r => r.Enrolment_ID)
                .IsUnique();

            // ==========================================
            // DECIMAL PRECISION FIX
            // ==========================================

            modelBuilder.Entity<Category>()
                .Property(c => c.Distance_Km)
                .HasPrecision(5, 2);

            modelBuilder.Entity<EventCategory>()
                .Property(ec => ec.Entry_Fee)
                .HasPrecision(10, 2);

            // ==========================================
            // FIX: Multiple Cascade Paths
            // ==========================================

            modelBuilder.Entity<Enrolment>()
                .HasOne(e => e.EventCategory)
                .WithMany(ec => ec.Enrolments)
                .HasForeignKey(e => e.Event_Category_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrolment>()
                .HasOne(e => e.Participant)
                .WithMany(u => u.Enrolments)
                .HasForeignKey(e => e.Participant_ID)
                .OnDelete(DeleteBehavior.Restrict);

            // ==========================================
            // SEED DATA
            // ==========================================

            modelBuilder.Entity<Role>().HasData(
                new Role { Role_ID = 1, Role_Name = "Organiser", Description = "Event organisers who create and manage events" },
                new Role { Role_ID = 2, Role_Name = "Participant", Description = "Participants who browse events and enrol in races" }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Category_ID = 1, Category_Name = "5km Fun Run", Description = "Family-friendly 5 kilometre run/walk", Distance_Km = 5.00m },
                new Category { Category_ID = 2, Category_Name = "10km Road Race", Description = "Challenging 10 kilometre road race", Distance_Km = 10.00m },
                new Category { Category_ID = 3, Category_Name = "21km Half Marathon", Description = "Competitive half marathon distance", Distance_Km = 21.10m },
                new Category { Category_ID = 4, Category_Name = "42km Marathon", Description = "Full marathon distance", Distance_Km = 42.20m },
                new Category { Category_ID = 5, Category_Name = "50km Cycle", Description = "Road cycling event - 50km", Distance_Km = 50.00m },
                new Category { Category_ID = 6, Category_Name = "100km Cycle", Description = "Long distance cycling event - 100km", Distance_Km = 100.00m }
            );
        }
    }
}