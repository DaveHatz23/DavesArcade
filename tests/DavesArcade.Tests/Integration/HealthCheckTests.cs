using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DavesArcade.Tests.Integration;

/// <summary>
/// Integration tests for the health check endpoint.
/// Verifies the /health endpoint returns correct status.
/// </summary>
public class HealthCheckTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HealthCheckTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheck_ReturnsHealthy()
    {
        // Arrange - test client already configured

        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task HealthCheck_ReturnsCorrectContentType()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/health");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/plain");
    }
}
