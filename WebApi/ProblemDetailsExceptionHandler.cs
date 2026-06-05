using CoreApp.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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

        // Dla wszystkich innych wyjątków zwracamy false, aby domyślny handler mógł je obsłużyć (zwracając 500)
        return false;
    }
}