using Microsoft.Extensions.Logging;
using Moq;
using VideoGame.Data.Repositories.Interfaces;
using VideoGame.Domain.Entities;
using VideoGame.Services.Implementations;
using System.Threading.Tasks;
using Xunit;

namespace VideoGame.Tests;

public class VideoGameCommandServiceTests
{
    private readonly Mock<IVideoGameWriteOnlyRepository> _writeRepositoryMock;
    private readonly Mock<IVideoGameReadOnlyRepository> _readRepositoryMock;
    private readonly Mock<ILogger<VideoGameCommandService>> _loggerMock;

    private readonly VideoGameCommandService _service;

    public VideoGameCommandServiceTests()
    {
        _writeRepositoryMock = new Mock<IVideoGameWriteOnlyRepository>();
        _readRepositoryMock = new Mock<IVideoGameReadOnlyRepository>();
        _loggerMock = new Mock<ILogger<VideoGameCommandService>>();

        _service = new VideoGameCommandService(
            _writeRepositoryMock.Object,
            _readRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task CreateGameAsync_WithValidGame_AddsGameAndReturnsGame()
    {
        // Arrange
        var game = new VideoGameModel
        {
            Id = 1,
            Title = "Super Mario",
            ReleaseDate = DateTime.UtcNow.AddYears(1)
        };

        // Act
        var result = await _service.CreateGameAsync(game);

        // Assert
        Assert.Equal(game, result);

        _writeRepositoryMock.Verify(
            x => x.AddAsync(game),
            Times.Once);
    }

    [Fact]
    public async Task CreateGameAsync_WithReleaseDateTooFarInFuture_ThrowsArgumentException()
    {
        // Arrange
        var game = new VideoGameModel
        {
            Id = 1,
            Title = "Future Game",
            ReleaseDate = DateTime.UtcNow.AddYears(6)
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateGameAsync(game));

        Assert.Equal(
            "Release date cannot be set too far in the future.",
            exception.Message);

        _writeRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<VideoGameModel>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateGameAsync_WhenIdDoesNotMatchGameId_ReturnsFalse()
    {
        // Arrange
        var game = new VideoGameModel
        {
            Id = 2,
            Title = "Super Mario",
            ReleaseDate = DateTime.UtcNow.AddYears(1)
        };

        // Act
        var result = await _service.UpdateGameAsync(1, game);

        // Assert
        Assert.False(result);

        _readRepositoryMock.Verify(
            x => x.ExistsAsync(It.IsAny<int>()),
            Times.Never);

        _writeRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<VideoGameModel>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateGameAsync_WhenGameDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var game = new VideoGameModel
        {
            Id = 1,
            Title = "Super Mario",
            ReleaseDate = DateTime.UtcNow.AddYears(1)
        };

        _readRepositoryMock
            .Setup(x => x.ExistsAsync(1))
            .ReturnsAsync(false);

        // Act
        var result = await _service.UpdateGameAsync(1, game);

        // Assert
        Assert.False(result);

        _readRepositoryMock.Verify(
            x => x.ExistsAsync(1),
            Times.Once);

        _writeRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<VideoGameModel>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateGameAsync_WhenGameExists_UpdatesGameAndReturnsTrue()
    {
        // Arrange
        var game = new VideoGameModel
        {
            Id = 1,
            Title = "Super Mario Updated",
            ReleaseDate = DateTime.UtcNow.AddYears(1)
        };

        _readRepositoryMock
            .Setup(x => x.ExistsAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _service.UpdateGameAsync(1, game);

        // Assert
        Assert.True(result);

        _readRepositoryMock.Verify(
            x => x.ExistsAsync(1),
            Times.Once);

        _writeRepositoryMock.Verify(
            x => x.UpdateAsync(game),
            Times.Once);
    }
}