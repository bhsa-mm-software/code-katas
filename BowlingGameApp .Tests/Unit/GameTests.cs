using Xunit;
using Moq;
using BowlingGameApp.Core.Services;
using BowlingGameApp.Core.Interfaces;
using BowlingGameApp.Core.Models;
using BowlingGameApp.Core.Exceptions;

namespace BowlingGameApp.Tests.Unit
{
    /// <summary>
    /// Unit tests for the Game class.
    /// Demonstrates proper testing with mocking and dependency injection.
    /// </summary>
    public class GameTests
    {
        private readonly Mock<IScoreCalculator> _mockScoreCalculator;
        private readonly Mock<IRollValidator> _mockRollValidator;
        private readonly Game _game;

        public GameTests()
        {
            _mockScoreCalculator = new Mock<IScoreCalculator>();
            _mockRollValidator = new Mock<IRollValidator>();
            _game = new Game(_mockScoreCalculator.Object, _mockRollValidator.Object);
        }

        [Fact]
        public void Constructor_WithNullScoreCalculator_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new Game(null!, _mockRollValidator.Object));
        }

        [Fact]
        public void Constructor_WithNullRollValidator_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new Game(_mockScoreCalculator.Object, null!));
        }

        [Fact]
        public void Roll_WithValidRoll_CallsValidator()
        {
            // Arrange
            _mockRollValidator.Setup(v => v.IsValidRoll(It.IsAny<int>(), It.IsAny<IReadOnlyList<int>>(), It.IsAny<int>()))
                             .Returns(true);

            // Act
            _game.Roll(5);

            // Assert
            _mockRollValidator.Verify(v => v.IsValidRoll(5, It.IsAny<IReadOnlyList<int>>(), 0), Times.Once);
        }

        [Fact]
        public void Roll_WithInvalidRoll_ThrowsInvalidRollException()
        {
            // Arrange
            _mockRollValidator.Setup(v => v.IsValidRoll(It.IsAny<int>(), It.IsAny<IReadOnlyList<int>>(), It.IsAny<int>()))
                             .Returns(false);

            // Act & Assert
            Assert.Throws<InvalidRollException>(() => _game.Roll(5));
        }

        [Fact]
        public void GetScore_CallsScoreCalculator()
        {
            // Arrange
            var expectedScore = 150;
            _mockScoreCalculator.Setup(c => c.CalculateTotalScore(It.IsAny<IReadOnlyList<Roll>>()))
                               .Returns(expectedScore);

            // Act
            var actualScore = _game.GetScore();

            // Assert
            Assert.Equal(expectedScore, actualScore);
            _mockScoreCalculator.Verify(c => c.CalculateTotalScore(It.IsAny<IReadOnlyList<Roll>>()), Times.Once);
        }

        [Fact]
        public void GetGameResult_CallsScoreCalculator()
        {
            // Arrange
            var expectedResult = new GameResult(300, 12, 0);
            _mockScoreCalculator.Setup(c => c.GetGameResult(It.IsAny<IReadOnlyList<Roll>>()))
                               .Returns(expectedResult);

            // Act
            var actualResult = _game.GetGameResult();

            // Assert
            Assert.Equal(expectedResult.TotalScore, actualResult.TotalScore);
            Assert.Equal(expectedResult.StrikesCount, actualResult.StrikesCount);
            Assert.Equal(expectedResult.SparesCount, actualResult.SparesCount);
        }

        [Fact]
        public void Roll_MultipleValidRolls_UpdatesGameState()
        {
            // Arrange
            _mockRollValidator.Setup(v => v.IsValidRoll(It.IsAny<int>(), It.IsAny<IReadOnlyList<int>>(), It.IsAny<int>()))
                             .Returns(true);
            _mockScoreCalculator.Setup(c => c.CalculateTotalScore(It.IsAny<IReadOnlyList<Roll>>()))
                               .Returns((IReadOnlyList<Roll> rolls) => rolls.Sum(r => r.PinsKnockedDown));

            // Act
            _game.Roll(3);
            _game.Roll(4);
            _game.Roll(7);

            var score = _game.GetScore();

            // Assert
            Assert.Equal(14, score); // 3 + 4 + 7
            _mockRollValidator.Verify(v => v.IsValidRoll(It.IsAny<int>(), It.IsAny<IReadOnlyList<int>>(), It.IsAny<int>()),
                                   Times.Exactly(3));
        }
    }
}