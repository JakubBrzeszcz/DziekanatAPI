using CoreApp.Module;
using Infrastructure;
using CoreApp.Interfaces;
using Infrastructure.Module;
using Infrastructure.Security;
using System.Text.Json.Serialization;
using WebApi.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Rejestracja nowego systemu obsługi wyjątków
        builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
        builder.Services.AddProblemDetails();

        // Rejestracja mechanizmów JWT i polityk autoryzacji
        builder.Services.AddSingleton<JwtSettings>();
        builder.Services.AddJwt(new JwtSettings(builder.Configuration));
        // Rejestracja serwisów (m.in. IAuthService)
        builder.Services.AddInfrastructure();

        // Rejestracja modułu Infrastructure (repozytoria EF, DbContext, UnitOfWork, Identity)
        builder.Services.AddUniversityEfModule(builder.Configuration);

        // Rejestracja modułu CoreApp (serwisy, AutoMapper, walidatory)
        builder.Services.AddStudentsModule(builder.Configuration);

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            // Serialize enums as strings in JSON
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        // Konfiguracja Swaggera z obsługą autoryzacji JWT
        builder.Services.AddSwaggerAuthorization();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            // Uruchamianie seederów w środowisku deweloperskim
            using var scope = app.Services.CreateScope();
            var seeders = scope.ServiceProvider
                .GetServices<IDataSeeder>()
                .OrderBy(s => s.Order);

            foreach (var seeder in seeders)
                await seeder.SeedAsync();
        }

        // Używamy nowego potoku obsługi wyjątków
        app.UseExceptionHandler();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
