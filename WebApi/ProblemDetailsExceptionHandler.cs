using CoreApp.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Middleware;

public class ProblemDetailsExceptionHandler(
    ProblemDetailsFactory factory, ILogger<ProblemDetailsExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        // Sprawdzamy, czy wyjątek jest jednym z naszych niestandardowych wyjątków "NotFound"
        if (exception is NotFoundException notFoundException)
        {
            logger.LogInformation("Obsługa wyjątku NotFoundException: {Message}", notFoundException.Message);

            var problemDetails = factory.CreateProblemDetails(
                context,
                StatusCodes.Status404NotFound, // Używamy poprawnego statusu 404
                title: "Nie znaleziono zasobu",
                detail: notFoundException.Message
            );

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            
            // Zwracamy true, informując, że wyjątek został obsłużony
            return true;
        }

        // Sprawdzamy, czy wyjątek to błąd bazy danych (np. naruszenie unikalności PESEL lub Email)
        if (exception is DbUpdateException dbUpdateException)
        {
            logger.LogWarning("Złapano błąd bazy danych (naruszenie unikalności): {Message}", dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);

            var problemDetails = factory.CreateProblemDetails(
                context,
                StatusCodes.Status409Conflict, // Używamy statusu 409 Conflict
                title: "Konflikt danych",
                detail: "Zasób z takimi unikalnymi danymi (np. podany PESEL lub Email) już istnieje w systemie."
            );

            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }

        // Sprawdzamy, czy wyjątek to błąd walidacji / zły argument (np. błędny PESEL rzucony przez Value Object)
        // AutoMapper często "owija" takie błędy w AutoMapperMappingException, więc zaglądamy też do InnerException
        var argEx = exception as ArgumentException ?? exception.InnerException as ArgumentException;
        if (argEx != null)
        {
            logger.LogWarning("Złapano błąd argumentu (walidacja): {Message}", argEx.Message);

            var problemDetails = factory.CreateProblemDetails(
                context,
                StatusCodes.Status400BadRequest, // Używamy statusu 400 Bad Request
                title: "Błąd walidacji danych",
                detail: argEx.Message
            );

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }

        // Dla wszystkich innych wyjątków zwracamy false, aby domyślny handler mógł je obsłużyć (zwracając 500)
        return false;
    }
}