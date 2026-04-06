using DavesArcade.Application.DTOs;
using DavesArcade.Application.Interfaces;
using DavesArcade.Application.Results;
using DavesArcade.Domain.Entities;

namespace DavesArcade.Infrastructure.Persistence.InMemory;

public class InMemoryGameRepository : IGameRepository
{
    private readonly List<Game> _games;

    public InMemoryGameRepository()
    {
        _games = GetSeedData();
    }

    private static List<Game> GetSeedData()
    {
        return new List<Game>
        {
            new Game
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d"),
                Name = "Halo Infinite",
                GenreId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Genre = new Genre { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Shooter" },
                Price = 59.99m,
                ReleaseDate = new DateOnly(2021, 12, 8),
                Description = "The legendary Halo series returns with the most expansive Master Chief story yet.",
                ImageUri = "/images/halo-infinite.jpg",
                LastUpdatedBy = "admin"
            },
            new Game
            {
                Id = Guid.Parse("b2c3d4e5-f6a7-4b5c-9d1e-2f3a4b5c6d7e"),
                Name = "Elden Ring",
                GenreId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Genre = new Genre { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "RPG" },
                Price = 69.99m,
                ReleaseDate = new DateOnly(2022, 2, 25),
                Description = "A dark fantasy action RPG from FromSoftware and George R.R. Martin.",
                ImageUri = "/images/elden-ring.jpg",
                LastUpdatedBy = "admin"
            },
            new Game
            {
                Id = Guid.Parse("c3d4e5f6-a7b8-4c5d-9e1f-3a4b5c6d7e8f"),
                Name = "Stardew Valley",
                GenreId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Genre = new Genre { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Simulation" },
                Price = 14.99m,
                ReleaseDate = new DateOnly(2016, 2, 26),
                Description = "Build the farm of your dreams in this charming countryside life RPG.",
                ImageUri = "/images/stardew-valley.jpg",
                LastUpdatedBy = "admin"
            },
            new Game
            {
                Id = Guid.Parse("d4e5f6a7-b8c9-4d5e-9f1a-4b5c6d7e8f9a"),
                Name = "Cyberpunk 2077",
                GenreId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Genre = new Genre { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "RPG" },
                Price = 49.99m,
                ReleaseDate = new DateOnly(2020, 12, 10),
                Description = "An open-world action-adventure story set in Night City, a megalopolis obsessed with power, glamour and body modification.",
                ImageUri = "/images/cyberpunk-2077.jpg",
                LastUpdatedBy = "admin"
            },
            new Game
            {
                Id = Guid.Parse("e5f6a7b8-c9d1-4e5f-9a1b-5c6d7e8f9a0b"),
                Name = "The Legend of Zelda: Tears of the Kingdom",
                GenreId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Genre = new Genre { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Adventure" },
                Price = 69.99m,
                ReleaseDate = new DateOnly(2023, 5, 12),
                Description = "An epic adventure across the land and skies of Hyrule awaits in this sequel to Breath of the Wild.",
                ImageUri = "/images/zelda-totk.jpg",
                LastUpdatedBy = "admin"
            }
        };
    }

    public Task<Result<IEnumerable<GameResultDto>>> GetAllAsync()
    {
        var gameDtos = _games.Select(game => new GameResultDto(
            game.Id,
            game.Name,
            game.Genre?.Name ?? "Unknown",
            game.Price,
            game.ReleaseDate,
            game.ImageUri,
            game.LastUpdatedBy
        ));

        return Task.FromResult(Result<IEnumerable<GameResultDto>>.Success(gameDtos));
    }

    public Task<Result<GameResultDto>> GetByIdAsync(Guid id)
    {
        var game = _games.FirstOrDefault(g => g.Id == id);

        if (game is null)
        {
            return Task.FromResult(Result<GameResultDto>.NotFound(
                "Game.NotFound", 
                $"Game with ID '{id}' was not found."));
        }

        var gameDto = new GameResultDto(
            game.Id,
            game.Name,
            game.Genre?.Name ?? "Unknown",
            game.Price,
            game.ReleaseDate,
            game.ImageUri,
            game.LastUpdatedBy
        );

        return Task.FromResult(Result<GameResultDto>.Success(gameDto));
    }

    public Task<Result<GameResultDto>> CreateAsync(CreateGameRequest createGameRequest)
    {
        // Validate genre exists (optional but good practice)
        var genreExists = _games.Any(g => g.Genre?.Id == createGameRequest.GenreId);

        if (!genreExists && createGameRequest.GenreId != Guid.Parse("11111111-1111-1111-1111-111111111111")
                         && createGameRequest.GenreId != Guid.Parse("22222222-2222-2222-2222-222222222222")
                         && createGameRequest.GenreId != Guid.Parse("33333333-3333-3333-3333-333333333333")
                         && createGameRequest.GenreId != Guid.Parse("44444444-4444-4444-4444-444444444444"))
        {
            return Task.FromResult(Result<GameResultDto>.Failure(
                "Game.InvalidGenre",
                $"Genre with ID '{createGameRequest.GenreId}' does not exist.",
                ErrorType.Validation));
        }

        var game = new Game
        {
            Id = Guid.NewGuid(),
            Name = createGameRequest.Name,
            GenreId = createGameRequest.GenreId,
            Genre = new Genre { Id = createGameRequest.GenreId, Name = GetGenreName(createGameRequest.GenreId) },
            Price = createGameRequest.Price,
            ReleaseDate = createGameRequest.ReleaseDate,
            Description = createGameRequest.Description,
            ImageUri = createGameRequest.ImageUri ?? "https://placehold.co/100",
            LastUpdatedBy = createGameRequest.LastUpdatedBy
        };

        _games.Add(game);

        var gameDto = new GameResultDto(
            game.Id,
            game.Name,
            game.Genre.Name,
            game.Price,
            game.ReleaseDate,
            game.ImageUri,
            game.LastUpdatedBy
        );

        return Task.FromResult(Result<GameResultDto>.Success(gameDto));
    }

    public Task<Result<GameResultDto>> UpdateAsync(Guid id, UpdateGameRequest updateGameRequest)
    {
        var game = _games.FirstOrDefault(g => g.Id == id);

        if (game is null)
        {
            return Task.FromResult(Result<GameResultDto>.NotFound(
                "Game.NotFound",
                $"Game with ID '{id}' was not found."));
        }

        // Validate genre exists
        if (updateGameRequest.GenreId != Guid.Parse("11111111-1111-1111-1111-111111111111")
            && updateGameRequest.GenreId != Guid.Parse("22222222-2222-2222-2222-222222222222")
            && updateGameRequest.GenreId != Guid.Parse("33333333-3333-3333-3333-333333333333")
            && updateGameRequest.GenreId != Guid.Parse("44444444-4444-4444-4444-444444444444"))
        {
            return Task.FromResult(Result<GameResultDto>.Failure(
                "Game.InvalidGenre",
                $"Genre with ID '{updateGameRequest.GenreId}' does not exist.",
                ErrorType.Validation));
        }

        // Update game properties
        game.Name = updateGameRequest.Name;
        game.GenreId = updateGameRequest.GenreId;
        game.Genre = new Genre { Id = updateGameRequest.GenreId, Name = GetGenreName(updateGameRequest.GenreId) };
        game.Price = updateGameRequest.Price;
        game.ReleaseDate = updateGameRequest.ReleaseDate;
        game.Description = updateGameRequest.Description;
        game.LastUpdatedBy = "system"; // or pass this in the request

        var gameDto = new GameResultDto(
            game.Id,
            game.Name,
            game.Genre.Name,
            game.Price,
            game.ReleaseDate,
            game.ImageUri,
            game.LastUpdatedBy
        );

        return Task.FromResult(Result<GameResultDto>.Success(gameDto));
    }

    public Task<Result<bool>> DeleteByIdAsync(Guid id)
    {
        var game = _games.FirstOrDefault(g => g.Id == id);

        if (game is null)
        {
            return Task.FromResult(Result<bool>.NotFound(
                "Game.NotFound",
                $"Game with ID '{id}' was not found."));
        }

        _games.Remove(game);
        return Task.FromResult(Result<bool>.Success(true));
    }

    private static string GetGenreName(Guid genreId) => genreId.ToString() switch
    {
        "11111111-1111-1111-1111-111111111111" => "Shooter",
        "22222222-2222-2222-2222-222222222222" => "RPG",
        "33333333-3333-3333-3333-333333333333" => "Simulation",
        "44444444-4444-4444-4444-444444444444" => "Adventure",
        _ => "Unknown"
    };
}