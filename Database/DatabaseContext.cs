using BMSAPI.Database.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BMSAPI.Database;

public class DatabaseContext : IdentityDbContext<User> {
    public DatabaseContext(DbContextOptions<DatabaseContext> options) :
        base(options) {
    }

    public DatabaseContext() {
    }

    public virtual DbSet<Child> Children { get; set; }
    public virtual DbSet<Diaper> Diapers { get; set; }
    public virtual DbSet<Feeding> Feedings { get; set; }
    public virtual DbSet<FeedingProfile> FeedingProfiles { get; set; }
    public virtual DbSet<Measurement> Measurements { get; set; }
    public virtual DbSet<Note> Notes { get; set; }
    public virtual DbSet<Sleep> Sleeps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (optionsBuilder.IsConfigured) return;

        const string appSettings = "appsettings.json";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(appSettings)
            .Build();

        var connectionString = configuration
            .GetConnectionString("DefaultConnection");

        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new SeedRoles());
        builder.ApplyConfiguration(new SeedAdminUser());
        builder.ApplyConfiguration(new SeedAdminRole());

        builder.Entity<User>().Ignore(u => u.PhoneNumberConfirmed);

        //Autogenerate keys
        builder.Entity<Child>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Diaper>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Feeding>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<FeedingProfile>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Measurement>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Note>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Sleep>().Property(x => x.Id).ValueGeneratedOnAdd();

        //Relationships
        builder.Entity<User>()
            .HasMany(x => x.Children)
            .WithMany(x => x.Parents);

        builder.Entity<Child>()
            .HasMany(x => x.Diapers)
            .WithOne(x => x.Child)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Child>()
            .HasMany(x => x.Feedings)
            .WithOne(x => x.Child)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Child>()
            .HasMany(x => x.FeedingProfiles)
            .WithOne(x => x.Child)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Child>()
            .HasMany(x => x.Measurements)
            .WithOne(x => x.Child)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Child>()
            .HasMany(x => x.Notes)
            .WithOne(x => x.Child)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Child>()
            .HasMany(x => x.Sleeps)
            .WithOne(x => x.Child)
            .OnDelete(DeleteBehavior.Cascade);

        //Required fields
        builder.Entity<User>().Property(x => x.Name).IsRequired();
        builder.Entity<User>().Property(x => x.Lastname).IsRequired();
        builder.Entity<User>().Property(x => x.UserName).IsRequired();
        builder.Entity<User>().Property(x => x.Email).IsRequired();

        builder.Entity<Child>().Property(x => x.Birthdate).IsRequired();
        builder.Entity<Child>().Property(x => x.Name).IsRequired();

        builder.Entity<Diaper>().Navigation(x => x.Child).IsRequired();
        builder.Entity<Diaper>().Property(x => x.Date).IsRequired();

        builder.Entity<Feeding>().Navigation(x => x.Child).IsRequired();
        builder.Entity<Feeding>().Property(x => x.Date).IsRequired();
        builder.Entity<Feeding>().Property(x => x.Amount).IsRequired();
        builder.Entity<Feeding>().Property(x => x.Breast).IsRequired();

        builder.Entity<FeedingProfile>().Navigation(x => x.Child).IsRequired();
        builder.Entity<FeedingProfile>().Property(x => x.Title).IsRequired();
        builder.Entity<FeedingProfile>().Property(x => x.TotalAmount).IsRequired();
        builder.Entity<FeedingProfile>().Property(x => x.TimesPrDay).IsRequired();

        builder.Entity<Measurement>().Navigation(x => x.Child).IsRequired();
        builder.Entity<Measurement>().Property(x => x.Date).IsRequired();
        builder.Entity<Measurement>().Property(x => x.Height).IsRequired();
        builder.Entity<Measurement>().Property(x => x.Length).IsRequired();
        builder.Entity<Measurement>().Property(x => x.Weight).IsRequired();
        builder.Entity<Measurement>().Property(x => x.HeadCircumference).IsRequired();

        builder.Entity<Note>().Navigation(x => x.Child).IsRequired();
        builder.Entity<Note>().Property(x => x.Date).IsRequired();
        builder.Entity<Note>().Property(x => x.Text).IsRequired();

        builder.Entity<Sleep>().Navigation(x => x.Child).IsRequired();
        builder.Entity<Sleep>().Property(x => x.Date).IsRequired();
        builder.Entity<Sleep>().Property(x => x.From).IsRequired();
        builder.Entity<Sleep>().Property(x => x.To).IsRequired();
        builder.Entity<Sleep>().Property(x => x.HeadPosition).IsRequired();
    }
}