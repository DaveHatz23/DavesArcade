using DavesArcade.Api.Extensions;
using DavesArcade.Application.Results;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System;

namespace DavesArcade.Tests.Unit.Api.Extensions;

public class ResultExtensionsTests
{
    #region ToHttpResult Tests

    [Fact]
    public void ToHttpResult_WithSuccessResult_ShouldReturnOk()
    {
        // Arrange
        var result = Result<string>.Success("test value");

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        httpResult.Should().BeOfType<Ok<string>>();
        var okResult = (Ok<string>)httpResult;
        okResult.Value.Should().Be("test value");
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public void ToHttpResult_WithSuccessResult_AndComplexObject_ShouldReturnOk()
    {
        // Arrange
        var gameDto = new { Id = Guid.NewGuid(), Name = "Test Game", Price = 29.99m };
        var result = Result<object>.Success(gameDto);

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        httpResult.Should().BeOfType<Ok<object>>();
        var okResult = (Ok<object>)httpResult;
        okResult.Value.Should().Be(gameDto);
    }

    [Fact]
    public void ToHttpResult_WithNotFoundError_ShouldReturnNotFound()
    {
        // Arrange
        var result = Result<string>.NotFound("Game.NotFound", "Game with ID '123' was not found");

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        httpResult.Should().BeOfType<NotFound<object>>();
        var notFoundResult = (NotFound<object>)httpResult;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var value = notFoundResult.Value as dynamic;
        ((string)value.error).Should().Be("Game with ID '123' was not found");
        ((string)value.code).Should().Be("Game.NotFound");
    }

    [Fact]
    public void ToHttpResult_WithValidationError_ShouldReturnBadRequest()
    {
        // Arrange
        var result = Result<string>.Failure("Game.InvalidGenre", "Invalid genre specified", ErrorType.Validation);

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        httpResult.Should().BeOfType<BadRequest<object>>();
        var badRequestResult = (BadRequest<object>)httpResult;
        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        var value = badRequestResult.Value as dynamic;
        ((string)value.error).Should().Be("Invalid genre specified");
        ((string)value.code).Should().Be("Game.InvalidGenre");
    }

    [Fact]
    public void ToHttpResult_WithConflictError_ShouldReturnConflict()
    {
        // Arrange
        var result = Result<string>.Conflict("Game.Duplicate", "A game with this name already exists");

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        httpResult.Should().BeOfType<Conflict<object>>();
        var conflictResult = (Conflict<object>)httpResult;
        conflictResult.StatusCode.Should().Be(StatusCodes.Status409Conflict);

        var value = conflictResult.Value as dynamic;
        ((string)value.error).Should().Be("A game with this name already exists");
        ((string)value.code).Should().Be("Game.Duplicate");
    }

    [Fact]
    public void ToHttpResult_WithUnknownErrorType_ShouldReturnProblemDetails()
    {
        // Arrange - Create a result with an undefined error type (cast to bypass enum validation)
        var error = new Error("Unknown.Error", "An unknown error occurred", (ErrorType)999);
        var result = Result<string>.Failure(error.Code, error.Description, error.Type);

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        httpResult.Should().BeOfType<ProblemHttpResult>();
        var problemResult = (ProblemHttpResult)httpResult;
        problemResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        problemResult.ProblemDetails.Detail.Should().Be("An unexpected error occurred.");
    }

    [Theory]
    [InlineData(ErrorType.NotFound, typeof(NotFound<object>))]
    [InlineData(ErrorType.Validation, typeof(BadRequest<object>))]
    [InlineData(ErrorType.Conflict, typeof(Conflict<object>))]
    public void ToHttpResult_WithDifferentErrorTypes_ShouldReturnCorrectHttpResultType(ErrorType errorType, Type expectedResultType)
    {
        // Arrange
        var result = Result<string>.Failure("Test.Code", "Test description", errorType);

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        httpResult.Should().BeOfType(expectedResultType);
    }

    #endregion

    #region ToCreatedResult Tests

    [Fact]
    public void ToCreatedResult_WithSuccessResult_ShouldReturnCreated()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var gameDto = new { Id = gameId, Name = "Test Game" };
        var result = Result<dynamic>.Success(gameDto);

        // Act
        var httpResult = result.ToCreatedResult(g => $"/games/{g.Id}");

        // Assert
        httpResult.Should().BeOfType<Created<dynamic>>();
        var createdResult = (Created<dynamic>)httpResult;
        createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdResult.Location.Should().Be($"/games/{gameId}");
        createdResult.Value.Should().Be(gameDto);
    }

    [Fact]
    public void ToCreatedResult_WithSuccessResult_AndComplexLocation_ShouldReturnCreatedWithCorrectLocation()
    {
        // Arrange
        var productDto = new { Id = 42, Category = "Electronics", Sku = "ABC123" };
        var result = Result<dynamic>.Success(productDto);

        // Act
        var httpResult = result.ToCreatedResult(p => $"/api/v1/categories/{p.Category}/products/{p.Id}");

        // Assert
        httpResult.Should().BeOfType<Created<dynamic>>();
        var createdResult = (Created<dynamic>)httpResult;
        createdResult.Location.Should().Be("/api/v1/categories/Electronics/products/42");
    }

    [Fact]
    public void ToCreatedResult_WithNotFoundError_ShouldReturnNotFound()
    {
        // Arrange
        var result = Result<string>.NotFound("Resource.NotFound", "Resource not found");

        // Act
        var httpResult = result.ToCreatedResult(s => $"/resources/{s}");

        // Assert
        httpResult.Should().BeOfType<NotFound<object>>();
        var notFoundResult = (NotFound<object>)httpResult;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public void ToCreatedResult_WithValidationError_ShouldReturnBadRequest()
    {
        // Arrange
        var result = Result<string>.Failure("Validation.Failed", "Invalid data", ErrorType.Validation);

        // Act
        var httpResult = result.ToCreatedResult(s => $"/resources/{s}");

        // Assert
        httpResult.Should().BeOfType<BadRequest<object>>();
    }

    [Fact]
    public void ToCreatedResult_WithConflictError_ShouldReturnConflict()
    {
        // Arrange
        var result = Result<string>.Conflict("Resource.Exists", "Resource already exists");

        // Act
        var httpResult = result.ToCreatedResult(s => $"/resources/{s}");

        // Assert
        httpResult.Should().BeOfType<Conflict<object>>();
    }

    #endregion

    #region ToNoContentResult Tests

    [Fact]
    public void ToNoContentResult_WithSuccessResult_ShouldReturnNoContent()
    {
        // Arrange
        var result = Result<bool>.Success(true);

        // Act
        var httpResult = result.ToNoContentResult();

        // Assert
        httpResult.Should().BeOfType<NoContent>();
        var noContentResult = (NoContent)httpResult;
        noContentResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public void ToNoContentResult_WithSuccessResult_AndAnyValue_ShouldReturnNoContent()
    {
        // Arrange
        var result = Result<string>.Success("This value is ignored");

        // Act
        var httpResult = result.ToNoContentResult();

        // Assert
        httpResult.Should().BeOfType<NoContent>();
    }

    [Fact]
    public void ToNoContentResult_WithNotFoundError_ShouldReturnNotFound()
    {
        // Arrange
        var result = Result<bool>.NotFound("Resource.NotFound", "Resource not found");

        // Act
        var httpResult = result.ToNoContentResult();

        // Assert
        httpResult.Should().BeOfType<NotFound<object>>();
        var notFoundResult = (NotFound<object>)httpResult;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public void ToNoContentResult_WithValidationError_ShouldReturnBadRequest()
    {
        // Arrange
        var result = Result<bool>.Failure("Validation.Failed", "Invalid data", ErrorType.Validation);

        // Act
        var httpResult = result.ToNoContentResult();

        // Assert
        httpResult.Should().BeOfType<BadRequest<object>>();
    }

    [Fact]
    public void ToNoContentResult_WithConflictError_ShouldReturnConflict()
    {
        // Arrange
        var result = Result<bool>.Conflict("Resource.Conflict", "Conflict occurred");

        // Act
        var httpResult = result.ToNoContentResult();

        // Assert
        httpResult.Should().BeOfType<Conflict<object>>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ToNoContentResult_WithSuccessResult_AndBooleanValues_ShouldReturnNoContent(bool value)
    {
        // Arrange
        var result = Result<bool>.Success(value);

        // Act
        var httpResult = result.ToNoContentResult();

        // Assert
        httpResult.Should().BeOfType<NoContent>();
    }

    #endregion

    #region Integration Tests - Realistic Scenarios

    [Fact]
    public void ToHttpResult_RealScenario_GetGameById_Success()
    {
        // Arrange - Simulating a successful game retrieval
        var gameDto = new
        {
            Id = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d"),
            Name = "Halo Infinite",
            Genre = "Shooter",
            Price = 59.99m
        };
        var result = Result<object>.Success(gameDto);

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        httpResult.Should().BeOfType<Ok<object>>();
        var okResult = (Ok<object>)httpResult;
        okResult.Value.Should().Be(gameDto);
    }

    [Fact]
    public void ToCreatedResult_RealScenario_CreateGame_Success()
    {
        // Arrange - Simulating a successful game creation
        var newGameId = Guid.NewGuid();
        var gameDto = new
        {
            Id = newGameId,
            Name = "New Game",
            Genre = "RPG",
            Price = 49.99m
        };
        var result = Result<dynamic>.Success(gameDto);

        // Act
        var httpResult = result.ToCreatedResult(g => $"/games/{g.Id}");

        // Assert
        httpResult.Should().BeOfType<Created<dynamic>>();
        var createdResult = (Created<dynamic>)httpResult;
        createdResult.Location.Should().Be($"/games/{newGameId}");
    }

    [Fact]
    public void ToNoContentResult_RealScenario_DeleteGame_Success()
    {
        // Arrange - Simulating a successful game deletion
        var result = Result<bool>.Success(true);

        // Act
        var httpResult = result.ToNoContentResult();

        // Assert
        httpResult.Should().BeOfType<NoContent>();
    }

    [Fact]
    public void ToHttpResult_RealScenario_UpdateGame_NotFound()
    {
        // Arrange - Simulating game not found during update
        var gameId = Guid.NewGuid();
        var result = Result<object>.NotFound(
            "Game.NotFound",
            $"Game with ID '{gameId}' was not found."
        );

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        httpResult.Should().BeOfType<NotFound<object>>();
        var notFoundResult = (NotFound<object>)httpResult;
        var value = notFoundResult.Value as dynamic;
        ((string)value.error).Should().Contain(gameId.ToString());
    }

    #endregion
}