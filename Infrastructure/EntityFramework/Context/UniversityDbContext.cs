using CoreApp.Entities;
using CoreApp.Enums;
using CoreApp.Interfaces;
using Infrastructure.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Security;
using CoreApp.ValueObjects;

namespace Infrastructure.EntityFramework.Context;

public class UniversityDbContext: IdentityDbContext<AppUser, AppRole, string>
{
    // Lista właściwości DbSet dla każdej encji aplikacji
    public DbSet<Student> Students { get; set;}
    public DbSet<Lecturer> Lecturers { get; set;}
    public DbSet<Course> Courses { get; set;}
    public DbSet<Grade> Grades { get; set;}
    public DbSet<DegreeProgram> DegreePrograms { get; set;}
    public DbSet<EnrollmentYear> EnrollmentYears { get; set;}
    public DbSet<RefreshToken> RefreshTokens { get; set;}
    public DbSet<GradeHistory> GradeHistories { get; set; }
    
    // Konstruktor używany przez mechanizm wstrzykiwania zależności (DI)
    public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options) { }

    // Pusty konstruktor jest potrzebny dla narzędzi deweloperskich, np. do tworzenia migracji
    public UniversityDbContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Konfiguracja bazy danych, jeśli nie została jeszcze skonfigurowana (np. przez DI)
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("data source=university.db");
        }

        // Ignorujemy fałszywe ostrzeżenie wywoływane przez dynamiczne generowanie hashy haseł w SeedData
        optionsBuilder.ConfigureWarnings(warnings => 
            warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // --------------------------------------
        // Cześć wspólna dla wszystkich projektów
        // --------------------------------------
        base.OnModelCreating(builder); // Wymagane przez Identity
        
        builder.Entity<AppUser>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(u => u.LastName).HasMaxLength(100).IsRequired();
            entity.Property(u => u.FullName).HasMaxLength(201).IsRequired();
            entity.Property(u => u.Department).HasMaxLength(100).IsRequired();
            entity.Property(u => u.Status).HasConversion<string>().IsRequired();
        });
        
        builder.Entity<AppRole>(entity =>
        {
            entity.ToTable("Roles");
            entity.Property(r => r.Description).HasMaxLength(256);
        });

        // Zmiana domyślnych nazw tabel Identity dla porządku
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        
        // -------------------------------------------
        // Cześć charakterystyczna dla projektu Dziekanat
        // -------------------------------------------
        
        builder.Entity<Student>(entity =>
        {
            entity.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(p => p.LastName).HasMaxLength(200).IsRequired();
            entity.Property(p => p.Email).HasMaxLength(200).IsRequired();
            entity.HasIndex(p => p.Email).IsUnique();
             entity.Property(p => p.NationalId)
                 .HasConversion(
                     v => v.Value,
                     v => new Pesel(v))
                 .HasMaxLength(11).IsRequired();
            entity.HasIndex(p => p.NationalId).IsUnique();
            entity.Property(p => p.Status).HasConversion<string>();
            
            entity.HasOne(s => s.DegreeProgram).WithMany();
            entity.HasMany(s => s.Grades).WithOne(g => g.Student).HasForeignKey("StudentId");
        });

        builder.Entity<Grade>(entity =>
        {
            entity.Property(g => g.GradeValue).HasConversion(
                v => v.Value(),
                v => GradeValueExtensions.FromDouble(v)
            ).IsRequired();
            entity.Property(g => g.Type).HasConversion<string>();
        });

        builder.Entity<DegreeProgram>(entity =>
        {
            entity.Property(p => p.Name).HasMaxLength(200).IsRequired();
            entity.Property(p => p.Code).HasMaxLength(20).IsRequired();
            entity.HasIndex(p => p.Code).IsUnique();
        });

        builder.Entity<Lecturer>(entity =>
        {
            entity.Property(l => l.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(l => l.LastName).HasMaxLength(200).IsRequired();
            entity.Property(l => l.Title).HasMaxLength(50);
            
            entity.Property(l => l.NationalId)
                .HasConversion(
                    v => v.Value,
                    v => new Pesel(v))
                .HasMaxLength(11).IsRequired();
        });

        builder.Entity<Course>(entity =>
        {
            entity.Property(c => c.Name).HasMaxLength(200).IsRequired();
        });
        
        builder.Entity<GradeHistory>(entity =>
        {
            entity.ToTable("GradeHistories");
            entity.HasKey(gh => gh.Id);
            entity.Property(gh => gh.Action).HasMaxLength(50);
            entity.HasOne(gh => gh.Grade).WithMany().HasForeignKey(gh => gh.GradeId).OnDelete(DeleteBehavior.Cascade);
        });
        
        // -------------------------------------------
        // Dane startowe (Seeding)
        // -------------------------------------------
        SeedData(builder);
    }

    private void SeedData(ModelBuilder builder)
    {
        // Role
        const string ADMIN_ROLE_ID = "2301D884-221A-4E7D-B509-0113DCC043E1";
        const string WORKER_ROLE_ID = "7D9B7113-A8F8-4035-99A7-A20DD400F6A3";

        builder.Entity<AppRole>().HasData(
            new AppRole(UserRole.Administrator.ToString(), "Rola administratora systemu")
            {
                Id = ADMIN_ROLE_ID,
                NormalizedName = UserRole.Administrator.ToString().ToUpperInvariant()
            },
            new AppRole(UserRole.DeaneryWorker.ToString(), "Rola pracownika dziekanatu")
            {
                Id = WORKER_ROLE_ID,
                NormalizedName = UserRole.DeaneryWorker.ToString().ToUpperInvariant()
            }
        );

        // Użytkownicy
        const string ADMIN_USER_ID = "B22698B8-42A2-4115-9631-1C2D1E2AC5F7";
        const string WORKER_USER_ID = "E244194A-4254-434A-8E9A-912784462A05";

        var hasher = new PasswordHasher<AppUser>();

        var adminUser = new AppUser
        {
            Id = ADMIN_USER_ID,
            FirstName = "Admin",
            LastName = "Główny",
            FullName = "Admin Główny",
            Department = "IT",
            Email = "admin@wsei.edu.pl",
            NormalizedEmail = "ADMIN@WSEI.EDU.PL",
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Status = SystemUserStatus.Active,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            EmailConfirmed = true,
            SecurityStamp = "A74268D7-17D3-4F8A-98CB-5A2BBEE2C741",
            PasswordHash = hasher.HashPassword(null!, "Admin123!")
        };

        var workerUser = new AppUser
        {
            Id = WORKER_USER_ID,
            FirstName = "Jan",
            LastName = "Kowalski",
            FullName = "Jan Kowalski",
            Department = "Dziekanat",
            Email = "jan.kowalski@wsei.edu.pl",
            NormalizedEmail = "JAN.KOWALSKI@WSEI.EDU.PL",
            UserName = "jankowalski",
            NormalizedUserName = "JANKOWALSKI",
            Status = SystemUserStatus.Active,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            EmailConfirmed = true,
            SecurityStamp = "B85379E8-28E4-5G9B-A9DC-6B3CCFF3D852",
            PasswordHash = hasher.HashPassword(null!, "Worker123!")
        };

        builder.Entity<AppUser>().HasData(adminUser, workerUser);

        // Przypisanie ról do użytkowników
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { RoleId = ADMIN_ROLE_ID, UserId = ADMIN_USER_ID },
            new IdentityUserRole<string> { RoleId = WORKER_ROLE_ID, UserId = WORKER_USER_ID }
        );
    }
}