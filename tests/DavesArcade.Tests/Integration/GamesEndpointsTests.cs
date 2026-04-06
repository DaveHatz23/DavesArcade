using System.Net;
using System.Net.Http.Json;
using DavesArcade.Application.DTOs;
using FluentAssertions;

namespace DavesArcade.Tests.Integration;

/// <summary>
/// Integration tests for all Games API endpoints.
/// Tests the complete HTTP request pipeline from request to response.
/// Each test creates a new factory instance for complete data isolation.
/// </summary>
public class GamesEndpointsTests : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public GamesEndpointsTests()
    {
        // Create a new factory for each test to ensure complete isolation
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    #region GET /games - Get All Games

    [Fact]
    public async Task GetGames_ReturnsOkStatus()
    {
        // Arrange - client already configured

        // Act
        var response = await _client.GetAsync("/games");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetGames_ReturnsAllGamesFromSeedData()
    {
        // Arrange
        var expectedGameCount = 5; // InMemoryGameRepository seeds 5 games

        // Act
        var response = await _client.GetAsync("/games");
        var games = await response.Content.ReadFromJsonAsync<List<GameResultDto>>();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        games.Should().NotBeNull();
        games.Should().HaveCount(expectedGameCount);
    }

    [Fact]
    public async Task GetGames_ReturnsGamesWithCorrectProperties()
    {
        // Act
        var response = await _client.GetAsync("/games");
        var games = await response.Content.ReadFromJsonAsync<List<GameResultDto>>();

        // Assert
        games.Should().NotBeNull();
        games.Should().AllSatisfy(game =>
        {
            game.Id.Should().NotBeEmpty();
            game.Name.Should().NotBeNullOrEmpty();
            game.Genre.Should().NotBeNullOrEmpty();
            game.Price.Should().BeGreaterThan(0);
            game.ReleaseDate.Should().NotBe(default);
            game.ImageUri.Should().NotBeNullOrEmpty();
            game.LastUpdatedBy.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task GetGames_ReturnsCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/games");

        // Assert
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
    }

    #endregion

    #region GET /games/{id} - Get Game By ID

    [Fact]
    public async Task GetGameById_WithValidId_ReturnsOkWithGame()
    {
        // Arrange - Using known seed data ID
        var validGameId = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d"); // Halo Infinite

        // Act
        var response = await _client.GetAsync($"/games/{validGameId}");
        var game = await response.Content.ReadFromJsonAsync<GameResultDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        game.Should().NotBeNull();
        game!.Id.Should().Be(validGameId);
        game.Name.Should().Be("Halo Infinite");
        game.Genre.Should().Be("Shooter");
    }

    [Fact]
    public async Task GetGameById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidGameId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/games/{invalidGameId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetGameById_WithInvalidId_ReturnsErrorMessage()
    {
        // Arrange
        var invalidGameId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/games/{invalidGameId}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        content.Should().Contain(invalidGameId.ToString());
        content.Should().Contain("not found");
    }

    [Theory]
    [InlineData("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d", "Halo Infinite", "Shooter")]
    [InlineData("b2c3d4e5-f6a7-4b5c-9d1e-2f3a4b5c6d7e", "Elden Ring", "RPG")]
    [InlineData("c3d4e5f6-a7b8-4c5d-9e1f-3a4b5c6d7e8f", "Stardew Valley", "Simulation")]
    [InlineData("d4e5f6a7-b8c9-4d5e-9f1a-4b5c6d7e8f9a", "Cyberpunk 2077", "RPG")]
    [InlineData("e5f6a7b8-c9d1-4e5f-9a1b-5c6d7e8f9a0b", "The Legend of Zelda: Tears of the Kingdom", "Adventure")]
    public async Task GetGameById_WithEachSeedGame_ReturnsCorrectGame(string gameIdString, string expectedName, string expectedGenre)
    {
        // Arrange
        var gameId = Guid.Parse(gameIdString);

        // Act
        var response = await _client.GetAsync($"/games/{gameId}");
        var game = await response.Content.ReadFromJsonAsync<GameResultDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        game.Should().NotBeNull();
        game!.Id.Should().Be(gameId);
        game.Name.Should().Be(expectedName);
        game.Genre.Should().Be(expectedGenre);
    }

    #endregion

    #region POST /games - Create Game

    [Fact]
    public async Task CreateGame_WithValidData_ReturnsCreatedStatus()
    {
        // Arrange
        var newGame = new CreateGameRequest(
            Name: "Test Game",
            GenreId: Guid.Parse("11111111-1111-1111-1111-111111111111"), // Shooter
            Price: 39.99m,
            ReleaseDate: new DateOnly(2024, 6, 15),
            Description: "A test game for integration testing",
            ImageUri: "/images/test-game.jpg",
            LastUpdatedBy: "integration-test"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/games", newGame);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateGame_WithValidData_ReturnsCreatedGameWithId()
    {
        // Arrange
        var newGame = new CreateGameRequest(
            Name: "New RPG Game",
            GenreId: Guid.Parse("22222222-2222-2222-2222-222222222222"), // RPG
            Price: 59.99m,
            ReleaseDate: new DateOnly(2024, 12, 1),
            Description: "An amazing new RPG",
            ImageUri: "/images/new-rpg.jpg",
            LastUpdatedBy: "test-user"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/games", newGame);
        var createdGame = await response.Content.ReadFromJsonAsync<GameResultDto>();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        createdGame.Should().NotBeNull();
        createdGame!.Id.Should().NotBeEmpty();
        createdGame.Name.Should().Be("New RPG Game");
        createdGame.Genre.Should().Be("RPG");
        createdGame.Price.Should().Be(59.99m);
        createdGame.ReleaseDate.Should().Be(new DateOnly(2024, 12, 1));
    }

    [Fact]
    public async Task CreateGame_WithValidData_ReturnsLocationHeader()
    {
        // Arrange
        var newGame = new CreateGameRequest(
            Name: "Location Test Game",
            GenreId: Guid.Parse("33333333-3333-3333-3333-333333333333"), // Simulation
            Price: 19.99m,
            ReleaseDate: new DateOnly(2024, 3, 15),
            Description: "Testing location header"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/games", newGame);
        var createdGame = await response.Content.ReadFromJsonAsync<GameResultDto>();

        // Assert
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.ToString().Should().Contain($"/games/{createdGame!.Id}");
    }

    [Fact]
    public async Task CreateGame_WithInvalidGenreId_ReturnsBadRequest()
    {
        // Arrange
        var invalidGame = new CreateGameRequest(
            Name: "Invalid Game",
            GenreId: Guid.NewGuid(), // Invalid genre ID
            Price: 29.99m,
            ReleaseDate: new DateOnly(2024, 1, 1),
            Description: "This should fail"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/games", invalidGame);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateGame_WithNullImageUri_UsesDefaultPlaceholder()
    {
        // Arrange
        var newGame = new CreateGameRequest(
            Name: "No Image Game",
            GenreId: Guid.Parse("44444444-4444-4444-4444-444444444444"), // Adventure
            Price: 49.99m,
            ReleaseDate: new DateOnly(2024, 8, 1),
            Description: "Game without custom image",
            ImageUri: null // Should use default
        );

        // Act
        var response = await _client.PostAsJsonAsync("/games", newGame);
        var createdGame = await response.Content.ReadFromJsonAsync<GameResultDto>();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        createdGame!.ImageUri.Should().Be("https://placehold.co/100");
    }

    #endregion

    #region PUT /games/{id} - Update Game

    [Fact]
    public async Task UpdateGame_WithValidData_ReturnsOkWithUpdatedGame()
    {
        // Arrange
        var existingGameId = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d"); // Halo Infinite
        var updateRequest = new UpdateGameRequest(
            Name: "Halo Infinite (Updated)",
            GenreId: Guid.Parse("11111111-1111-1111-1111-111111111111"), // Shooter
            Price: 49.99m,
            ReleaseDate: new DateOnly(2021, 12, 8),
            Description: "Updated description with new content"
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/games/{existingGameId}", updateRequest);
        var updatedGame = await response.Content.ReadFromJsonAsync<GameResultDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedGame.Should().NotBeNull();
        updatedGame!.Id.Should().Be(existingGameId);
        updatedGame.Name.Should().Be("Halo Infinite (Updated)");
        updatedGame.Price.Should().Be(49.99m);
        updatedGame.LastUpdatedBy.Should().Be("system");
    }

    [Fact]
    public async Task UpdateGame_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidGameId = Guid.NewGuid();
        var updateRequest = new UpdateGameRequest(
            Name: "Non-existent Game",
            GenreId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Price: 29.99m,
            ReleaseDate: new DateOnly(2024, 1, 1),
            Description: "This should not be found"
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/games/{invalidGameId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateGame_WithInvalidGenreId_ReturnsBadRequest()
    {
        // Arrange
        var existingGameId = Guid.Parse("b2c3d4e5-f6a7-4b5c-9d1e-2f3a4b5c6d7e"); // Elden Ring
        var updateRequest = new UpdateGameRequest(
            Name: "Elden Ring Updated",
            GenreId: Guid.NewGuid(), // Invalid genre
            Price: 59.99m,
            ReleaseDate: new DateOnly(2022, 2, 25),
            Description: "Invalid genre update"
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/games/{existingGameId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateGame_PreservesImageUri()
    {
        // Arrange
        var gameId = Guid.Parse("c3d4e5f6-a7b8-4c5d-9e1f-3a4b5c6d7e8f"); // Stardew Valley
        var originalImageUri = "/images/stardew-valley.jpg";
        
        var updateRequest = new UpdateGameRequest(
            Name: "Stardew Valley (Updated)",
            GenreId: Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Price: 19.99m,
            ReleaseDate: new DateOnly(2016, 2, 26),
            Description: "Updated description"
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/games/{gameId}", updateRequest);
        var updatedGame = await response.Content.ReadFromJsonAsync<GameResultDto>();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        updatedGame!.ImageUri.Should().Be(originalImageUri); // Should be preserved
    }

    #endregion

    #region DELETE /games/{id} - Delete Game

    [Fact]
    public async Task DeleteGame_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var gameId = Guid.Parse("d4e5f6a7-b8c9-4d5e-9f1a-4b5c6d7e8f9a"); // Cyberpunk 2077

        // Act
        var response = await _client.DeleteAsync($"/games/{gameId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteGame_WithValidId_RemovesGameFromRepository()
    {
        // Arrange
        var gameId = Guid.Parse("e5f6a7b8-c9d1-4e5f-9a1b-5c6d7e8f9a0b"); // Zelda

        // Act
        var deleteResponse = await _client.DeleteAsync($"/games/{gameId}");
        var getResponse = await _client.GetAsync($"/games/{gameId}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteGame_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidGameId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/games/{invalidGameId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteGame_CalledTwice_ReturnsNotFoundOnSecondCall()
    {
        // Arrange
        var gameId = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d"); // Halo Infinite

        // Act
        var firstDeleteResponse = await _client.DeleteAsync($"/games/{gameId}");
        var secondDeleteResponse = await _client.DeleteAsync($"/games/{gameId}");

        // Assert
        firstDeleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        secondDeleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region End-to-End Scenarios

    [Fact]
    public async Task CompleteGameLifecycle_CreateUpdateDelete_WorksCorrectly()
    {
        // Arrange
        var createRequest = new CreateGameRequest(
            Name: "Lifecycle Test Game",
            GenreId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Price: 39.99m,
            ReleaseDate: new DateOnly(2024, 6, 1),
            Description: "Testing complete lifecycle"
        );

        // Act & Assert - Create
        var createResponse = await _client.PostAsJsonAsync("/games", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdGame = await createResponse.Content.ReadFromJsonAsync<GameResultDto>();
        createdGame.Should().NotBeNull();

        // Act & Assert - Read
        var getResponse = await _client.GetAsync($"/games/{createdGame!.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act & Assert - Update
        var updateRequest = new UpdateGameRequest(
            Name: "Updated Lifecycle Game",
            GenreId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Price: 29.99m,
            ReleaseDate: new DateOnly(2024, 6, 1),
            Description: "Updated description"
        );
        var updateResponse = await _client.PutAsJsonAsync($"/games/{createdGame.Id}", updateRequest);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act & Assert - Delete
        var deleteResponse = await _client.DeleteAsync($"/games/{createdGame.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion
        var finalGetResponse = await _client.GetAsync($"/games/{createdGame.Id}");
        finalGetResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion
}
