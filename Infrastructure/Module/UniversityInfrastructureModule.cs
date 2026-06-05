using CoreApp.Repositories;
using CoreApp.UnitOfWork;
using Infrastructure.EntityFramework.Context;
using Infrastructure.EntityFramework.Entities;
using Infrastructure.EntityFramework.Repositories;
using Infrastructure.EntityFramework.UnitOfWork;
using Infrastructure.Memory;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Module;

public static class UniversityInfrastructureModule
{
    /// <summary>
    /// Rejestruje komponenty warstwy Infrastructure korzystające z Entity Framework Core.
    /// </summary>
    public static IServiceCollection AddUniversityEfModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Repositories
        services.AddScoped<IStudentRepository, EfStudentRepository>();
        services.AddScoped<IDegreeProgramRepository, EfDegreeProgramRepository>();
        services.AddScoped<ILecturerRepository, EfLecturerRepository>();
        services.AddScoped<ICourseRepository, EfCourseRepository>();
        services.AddScoped<IGradeRepository, EfGradeRepository>();

        // Unit of Work
        services.AddScoped<IUniversityUnitOfWork, EfUniversityUnitOfWork>();

        // DbContext
        services.AddDbContext<UniversityDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("UniversityDb")));

        // Identity
        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<UniversityDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    /// <summary>
    /// Rejestruje komponenty warstwy Infrastructure korzystające z implementacji w pamięci.
    /// </summary>
    public static IServiceCollection AddUniversityMemoryModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Repositories
        services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
        services.AddSingleton<IDegreeProgramRepository, InMemoryDegreeProgramRepository>();
        services.AddSingleton<ILecturerRepository, InMemoryLecturerRepository>();
        services.AddSingleton<ICourseRepository, InMemoryCourseRepository>();
        services.AddSingleton<IGradeRepository, InMemoryGradeRepository>();

        // Unit of Work
        services.AddSingleton<IUniversityUnitOfWork, MemoryUniversityUnitOfWork>();

        return services;
    }
}