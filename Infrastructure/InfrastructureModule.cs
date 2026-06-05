using CoreApp.Authorization;
using CoreApp.Enums;
using CoreApp.Interfaces;
using Infrastructure.Security;
using Infrastructure.EntityFramework.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDataSeeder, UniversityDataSeeder>();
        return services;
    }
    
    public static IServiceCollection AddJwt(this IServiceCollection services, JwtSettings jwtOptions)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = jwtOptions.GetSymmetricKey(),
                        ClockSkew = TimeSpan.Zero // brak tolerancji czasu
                    };
                }
            );
        services.AddAuthorization(options =>
        {
            // Polityki oparte o role
            options.AddPolicy(AppPolicies.AdminOnly.ToString(), policy =>
                policy.RequireRole(UserRole.Administrator.ToString()));
            
            options.AddPolicy(AppPolicies.DeaneryWorkerOnly.ToString(), policy =>
                policy.RequireRole(UserRole.DeaneryWorker.ToString()));

            // Polityka złożona — wymaga roli i aktywnego konta
            options.AddPolicy(AppPolicies.ActiveUser.ToString(), policy =>
                policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("status", SystemUserStatus.Active.ToString()));

            // Polityka oparta o dział (używamy "IT", bo istnieje w danych startowych)
            options.AddPolicy(AppPolicies.ITDepartmentOnly.ToString(), policy =>
                policy.RequireClaim("department", "IT"));

            // Domyślna polityka — każdy zalogowany użytkownik
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            // Polityka fallback — stosowana gdy brak atrybutu [Authorize] na endpoincie
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
        return services;
    }
}