using BMSAPI.Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMSAPI.Database;

public class DbSeeder {
}

public class SeedRoles : IEntityTypeConfiguration<IdentityRole> {
    public void Configure(EntityTypeBuilder<IdentityRole> builder) {
        builder.HasData(
            new IdentityRole {
                Id = "-2",
                Name = "User",
                NormalizedName = "USER"
            },
            new IdentityRole {
                Id = "-1",
                Name = "Admin",
                NormalizedName = "ADMIN"
            }
        );
    }
}

public class SeedAdminRole : IEntityTypeConfiguration<IdentityUserRole<string>> {
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder) {
        var assignRole = new IdentityUserRole<string> {
            RoleId = "-1",
            UserId = "-1"
        };

        builder.HasData(assignRole);

        var assignRole2 = new IdentityUserRole<string> {
            RoleId = "-2",
            UserId = "-2"
        };

        builder.HasData(assignRole2);

        var assignRole3 = new IdentityUserRole<string> {
            RoleId = "-2",
            UserId = "-3"
        };

        builder.HasData(assignRole3);
    }
}

public class SeedAdminUser : IEntityTypeConfiguration<User> {
    public void Configure(EntityTypeBuilder<User> builder) {
        var admin = new User {
            Id = "-1",
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Name = "ADMIN",
            Lastname = "ADMIN",
            NormalizedEmail = "ADMIN",
            Email = "ADMIN",
            Country = "ADMIN",
            City = "ADMIN",
            Zip = "ADMIN",
            Street = "ADMIN",
            StreetNumber = "ADMIN",
            Floor = "ADMIN",
            PhoneNumber = "ADMIN",
        };
        admin.PasswordHash = PwGenerator(admin);
        builder.HasData(admin);
    }

    private string PwGenerator(User user) {
        var passHash = new PasswordHasher<User>();
        return passHash.HashPassword(user, "admin");
    }
}

public class SeedFamilyOneMom : IEntityTypeConfiguration<User> {
    public void Configure(EntityTypeBuilder<User> builder) {
        var mom = new User {
            Id = "-2",
            UserName = "momone",
            NormalizedUserName = "MOMONE",
            Name = "momone",
            Lastname = "momone",
            NormalizedEmail = "MOMONE",
            Email = "momone",
            Country = "momone",
            City = "momone",
            Zip = "momone",
            Street = "momone",
            StreetNumber = "momone",
            Floor = "momone",
            PhoneNumber = "momone",
        };
        mom.PasswordHash = PwGenerator(mom);
        builder.HasData(mom);
    }

    private string PwGenerator(User user) {
        var passHash = new PasswordHasher<User>();
        return passHash.HashPassword(user, "momone");
    }
}

public class SeedFamilyOneDad : IEntityTypeConfiguration<User> {
    public void Configure(EntityTypeBuilder<User> builder) {
        var dad = new User {
            Id = "-3",
            UserName = "dadone",
            NormalizedUserName = "DADONE",
            Name = "dadone",
            Lastname = "dadone",
            NormalizedEmail = "DADONE",
            Email = "dadone",
            Country = "dadone",
            City = "dadone",
            Zip = "dadone",
            Street = "dadone",
            StreetNumber = "dadone",
            Floor = "dadone",
            PhoneNumber = "dadone",
        };
        dad.PasswordHash = PwGenerator(dad);
        builder.HasData(dad);
    }

    private string PwGenerator(User user) {
        var passHash = new PasswordHasher<User>();
        return passHash.HashPassword(user, "dadone");
    }
}