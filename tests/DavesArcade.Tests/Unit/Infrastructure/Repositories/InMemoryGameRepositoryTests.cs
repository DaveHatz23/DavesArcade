using DavesArcade.Application.DTOs;
using DavesArcade.Application.Results;
using DavesArcade.Infrastructure.Persistence.InMemory;
using FluentAssertions;
using System;
using System.Threading.Tasks;

namespace DavesArcade.Tests.Unit.Infrastructure.Repositories;

public class InMemoryGameRepositoryTests
{
    private readonly InMemoryGameRepository _repository;

    public InMemoryGameRepositoryTests()
    {
        _repository = new InMemoryGameRepository();
    }

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_ShouldReturnSuccess_WithAllGames()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(5); // Based on seed data
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnGamesWithCorrectData()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        var games = result.Value!.ToList();
        games.Should().Contain(g => g.Name == "Halo Infinite" && g.Genre == "Shooter");
        games.Should().Contain(g => g.Name == "Elden Ring" && g.Genre == "RPG");
        games.Should().Contain(g => g.Name == "Stardew Valley" && g.Genre == "Simulation");
        games.Should().Contain(g => g.Name == "Cyberpunk 2077" && g.Genre == "RPG");
        games.Should().Contain(g => g.Name == "The Legend of Zelda: Tears of the Kingdom" && g.Genre == "Adventure");
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnSuccess_WithCorrectGame()
    {
        // Arrange
        var validId = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d"); // Halo Infinite

        // Act
        var result = await _repository.GetByIdAsync(validId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Id.Should().Be(validId);
        result.Value.Name.Should().Be("Halo Infinite");
        result.Value.Genre.Should().Be("Shooter");
        result.Value.Price.Should().Be(59.99m);
        result.Value.ReleaseDate.Should().Be(new DateOnly(2021, 12, 8));
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(invalidId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().NotBeNull();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        result.Error.Code.Should().Be("Game.NotFound");
        result.Error.Description.Should().Contain(invalidId.ToString());
    }

    [Fact]
    public async Task GetByIdAsync_WithEachSeedGame_ShouldReturnCorrectGame()
    {
        // Arrange - Test all seed game IDs
        var testCases = new[]
        {
            (Id: Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d"), Name: "Halo Infinite"),
            (Id: Guid.Parse("b2c3d4e5-f6a7-4b5c-9d1e-2f3a4b5c6d7e"), Name: "Elden Ring"),
            (Id: Guid.Parse("c3d4e5f6-a7b8-4c5d-9e1f-3a4b5c6d7e8f"), Name: "Stardew Valley"),
            (Id: Guid.Parse("d4e5f6a7-b8c9-4d5e-9f1a-4b5c6d7e8f9a"), Name: "Cyberpunk 2077"),
            (Id: Guid.Parse("e5f6a7b8-c9d1-4e5f-9a1b-5c6d7e8f9a0b"), Name: "The Legend of Zelda: Tears of the Kingdom")
        };

        foreach (var testCase in testCases)
        {
            // Act
            var result = await _repository.GetByIdAsync(testCase.Id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value!.Name.Should().Be(testCase.Name);
        }
    }

    #endregion

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldReturnSuccess_WithNewGame()
    {
        // Arrange
        var createRequest = new CreateGameRequest(
            "Test Game",
            Guid.Parse("11111111-1111-1111-1111-111111111111"), // Shooter
            29.99m,
            new DateOnly(2024, 1, 1),
            "A test game description",
            "https://test.com/image.jpg",
            "testuser"
        );

        // Act
        var result = await _repository.CreateAsync(createRequest);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Name.Should().Be("Test Game");
        result.Value.Genre.Should().Be("Shooter");
        result.Value.Price.Should().Be(29.99m);
        result.Value.ReleaseDate.Should().Be(new DateOnly(2024, 1, 1));
        result.Value.ImageUri.Should().Be("https://test.com/image.jpg");
        result.Value.LastUpdatedBy.Should().Be("testuser");
        result.Value.Id.Should().NotBeEmpty();
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WithNullImageUri_ShouldUseDefaultPlaceholder()
    {
        // Arrange
        var createRequest = new CreateGameRequest(
            "Test Game Without Image",
            Guid.Parse("22222222-2222-2222-2222-222222222222"), // RPG
            19.99m,
            new DateOnly(2024, 2, 1),
            "A test game without image"
        );

        // Act
        var result = await _repository.CreateAsync(createRequest);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.ImageUri.Should().Be("https://placehold.co/100");
    }

    [Theory]
    [InlineData("11111111-1111-1111-1111-111111111111", "Shooter")]
    [InlineData("22222222-2222-2222-2222-222222222222", "RPG")]
    [InlineData("33333333-3333-3333-3333-333333333333", "Simulation")]
    [InlineData("44444444-4444-4444-4444-444444444444", "Adventure")]
    public async Task CreateAsync_WithValidGenreIds_ShouldReturnSuccess_WithCorrectGenreName(string genreIdString, string expectedGenreName)
    {
        // Arrange
        var genreId = Guid.Parse(genreIdString);
        var createRequest = new CreateGameRequest(
            "Genre Test Game",
            genreId,
            29.99m,
            new DateOnly(2024, 1, 1),
            "Testing genre assignment"
        );

        // Act
        var result = await _repository.CreateAsync(createRequest);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Genre.Should().Be(expectedGenreName);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidGenreId_ShouldReturnValidationError()
    {
        // Arrange
        var invalidGenreId = Guid.NewGuid();
        var createRequest = new CreateGameRequest(
            "Test Game",
            invalidGenreId,
            29.99m,
            new DateOnly(2024, 1, 1),
            "A test game with invalid genre"
        );

        // Act
        var result = await _repository.CreateAsync(createRequest);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().NotBeNull();
        result.Error!.Type.Should().Be(ErrorType.Validation);
        result.Error.Code.Should().Be("Game.InvalidGenre");
        result.Error.Description.Should().Contain(invalidGenreId.ToString());
    }

    [Fact]
    public async Task CreateAsync_ShouldAddGameToRepository()
    {
        // Arrange
        var createRequest = new CreateGameRequest(
            "New Game",
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            39.99m,
            new DateOnly(2024, 3, 1),
            "Test persistence"
        );

        // Act
        var createResult = await _repository.CreateAsync(createRequest);
        var getAllResult = await _repository.GetAllAsync();

        // Assert
        createResult.IsSuccess.Should().BeTrue();
        getAllResult.Value.Should().HaveCount(6); // 5 seed + 1 new
        getAllResult.Value.Should().Contain(g => g.Name == "New Game");
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_WithValidId_ShouldReturnSuccess_WithUpdatedGame()
    {
        // Arrange
        var existingId = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d"); // Halo Infinite
        var updateRequest = new UpdateGameRequest(
            "Halo Infinite Updated",
            Guid.Parse("22222222-2222-2222-2222-222222222222"), // Change to RPG
            39.99m,
            new DateOnly(2024, 1, 1),
            "Updated description"
        );

        // Act
        var result = await _repository.UpdateAsync(existingId, updateRequest);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Id.Should().Be(existingId);
        result.Value.Name.Should().Be("Halo Infinite Updated");
        result.Value.Genre.Should().Be("RPG");
        result.Value.Price.Should().Be(39.99m);
        result.Value.ReleaseDate.Should().Be(new DateOnly(2024, 1, 1));
        result.Value.LastUpdatedBy.Should().Be("system");
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        var updateRequest = new UpdateGameRequest(
            "Test",
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            29.99m,
            new DateOnly(2024, 1, 1),
            "Test"
        );

        // Act
        var result = await _repository.UpdateAsync(invalidId, updateRequest);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().NotBeNull();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        result.Error.Code.Should().Be("Game.NotFound");
        result.Error.Description.Should().Contain(invalidId.ToString());
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidGenreId_ShouldReturnValidationError()
    {
        // Arrange
        var existingId = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d");
        var invalidGenreId = Guid.NewGuid();
        var updateRequest = new UpdateGameRequest(
            "Test",
            invalidGenreId,
            29.99m,
            new DateOnly(2024, 1, 1),
            "Test"
        );

        // Act
        var result = await _repository.UpdateAsync(existingId, updateRequest);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.Validation);
        result.Error.Code.Should().Be("Game.InvalidGenre");
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        // Arrange
        var gameId = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d");
        var updateRequest = new UpdateGameRequest(
            "Updated Name",
            Guid.Parse("33333333-3333-3333-3333-333333333333"), // Simulation
            99.99m,
            new DateOnly(2025, 12, 31),
            "Persistence test"
        );

        // Act
        await _repository.UpdateAsync(gameId, updateRequest);
        var getResult = await _repository.GetByIdAsync(gameId);

        // Assert
        getResult.Value!.Name.Should().Be("Updated Name");
        getResult.Value.Genre.Should().Be("Simulation");
        getResult.Value.Price.Should().Be(99.99m);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPreserveImageUri()
    {
        // Arrange
        var gameId = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d");
        var originalImageUri = "/images/halo-infinite.jpg";
        var updateRequest = new UpdateGameRequest(
            "Updated Name",
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            59.99m,
            new DateOnly(2024, 1, 1),
            "Test"
        );

        // Act
        var result = await _repository.UpdateAsync(gameId, updateRequest);

        // Assert
        result.Value!.ImageUri.Should().Be(originalImageUri); // Should not change
    }

    #endregion

    #region DeleteByIdAsync Tests

    [Fact]
    public async Task DeleteByIdAsync_WithValidId_ShouldReturnSuccess()
    {
        // Arrange
        var validId = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d"); // Halo Infinite

        // Act
        var result = await _repository.DeleteByIdAsync(validId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task DeleteByIdAsync_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var result = await _repository.DeleteByIdAsync(invalidId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        result.Error.Code.Should().Be("Game.NotFound");
        result.Error.Description.Should().Contain(invalidId.ToString());
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldRemoveGameFromRepository()
    {
        // Arrange
        var gameId = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d");

        // Act
        var deleteResult = await _repository.DeleteByIdAsync(gameId);
        var getResult = await _repository.GetByIdAsync(gameId);
        var getAllResult = await _repository.GetAllAsync();

        // Assert
        deleteResult.IsSuccess.Should().BeTrue();
        getResult.IsSuccess.Should().BeFalse(); // Game should not be found
        getResult.Error!.Type.Should().Be(ErrorType.NotFound);
        getAllResult.Value.Should().HaveCount(4); // 5 seed - 1 deleted
        getAllResult.Value.Should().NotContain(g => g.Id == gameId);
    }

    [Fact]
    public async Task DeleteByIdAsync_CalledTwice_ShouldReturnNotFoundOnSecondCall()
    {
        // Arrange
        var gameId = Guid.Parse("b2c3d4e5-f6a7-4b5c-9d1e-2f3a4b5c6d7e");

        // Act
        var firstDelete = await _repository.DeleteByIdAsync(gameId);
        var secondDelete = await _repository.DeleteByIdAsync(gameId);

        // Assert
        firstDelete.IsSuccess.Should().BeTrue();
        secondDelete.IsSuccess.Should().BeFalse();
        secondDelete.Error!.Type.Should().Be(ErrorType.NotFound);
    }

    #endregion
}