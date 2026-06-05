using System.Net;
using System.Net.Http.Json;
using CoreApp.Dto;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApi;
using Xunit;

namespace UnitTest.IntegrationTests;

public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(WebApplicationFactory<Program> factory)
    {
        // Tworzy wirtualnego klienta HTTP połączonego z naszym API w pamięci
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetMe_WithoutToken_ReturnsUnauthorized()
    {
        // Act - Wysyłamy zapytanie bez tokenu JWT
        var response = await _client.GetAsync("/api/auth/me");

        // Assert - Oczekujemy statusu 401 Unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOkAndTokens()
    {
        // Arrange - Przygotowujemy poprawne dane logowania z naszego Seedera
        var loginDto = new LoginDto
        {
            Email = "admin@wsei.edu.pl",
            Password = "Admin123!"
        };

        // Act - Wysyłamy zapytanie logowania
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert - Oczekujemy statusu 200 OK i wygenerowanych tokenów
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        Assert.NotNull(authResponse);
        Assert.NotEmpty(authResponse.AccessToken);
        Assert.NotEmpty(authResponse.RefreshToken);
        Assert.NotNull(authResponse.User);
    }
}