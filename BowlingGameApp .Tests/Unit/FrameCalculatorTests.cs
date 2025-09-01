using Xunit;
using BowlingGameApp.Core.Services.FrameCalculators;
using BowlingGameApp.Core.Models;

namespace BowlingGameApp.Tests.Unit
{
    /// <summary>
    /// Unit tests for frame calculators.
    /// Tests each calculator in isolation following the Single Responsibility Principle.
    /// </summary>
    public class FrameCalculatorTests
    {
        [Fact]
        public void StrikeCalculator_CanHandle_Strike_ReturnsTrue()
        {
            // Arrange
            var calculator = new StrikeCalculator();

            // Act
            var canHandle = calculator.CanHandle(FrameType.Strike);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void StrikeCalculator_CanHandle_NonStrike_ReturnsFalse()
        {
            // Arrange
            var calculator = new StrikeCalculator();

            // Act & Assert
            Assert.False(calculator.CanHandle(FrameType.Spare));
            Assert.False(calculator.CanHandle(FrameType.Open));
        }

        [Fact]
        public void StrikeCalculator_CalculateScore_WithBonus_ReturnsCorrectScore()
        {
            // Arrange
            var calculator = new StrikeCalculator();
            var frame = new Frame(FrameType.Strike, 1, 0);
            var rolls = new List<Roll> { 10, 3, 4 }; // Strike + 3 + 4 bonus

            // Act
            var score = calculator.CalculateScore(frame, rolls);

            // Assert
            Assert.Equal(17, score); // 10 + 3 + 4
        }

        [Fact]
        public void StrikeCalculator_CalculateScore_WithWrongFrameType_ThrowsArgumentException()
        {
            // Arrange
            var calculator = new StrikeCalculator();
            var frame = new Frame(FrameType.Spare, 1, 0);
            var rolls = new List<Roll> { 7, 3, 5 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.CalculateScore(frame, rolls));
        }

        [Fact]
        public void SpareCalculator_CanHandle_Spare_ReturnsTrue()
        {
            // Arrange
            var calculator = new SpareCalculator();

            // Act
            var canHandle = calculator.CanHandle(FrameType.Spare);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void SpareCalculator_CalculateScore_WithBonus_ReturnsCorrectScore()
        {
            // Arrange
            var calculator = new SpareCalculator();
            var frame = new Frame(FrameType.Spare, 1, 0);
            var rolls = new List<Roll> { 7, 3, 5 }; // Spare (7+3) + 5 bonus

            // Act
            var score = calculator.CalculateScore(frame, rolls);

            // Assert
            Assert.Equal(15, score); // 10 + 5
        }

        [Fact]
        public void SpareCalculator_CalculateScore_WithWrongFrameType_ThrowsArgumentException()
        {
            // Arrange
            var calculator = new SpareCalculator();
            var frame = new Frame(FrameType.Strike, 1, 0);
            var rolls = new List<Roll> { 10, 3, 4 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.CalculateScore(frame, rolls));
        }

        [Fact]
        public void OpenFrameCalculator_CanHandle_Open_ReturnsTrue()
        {
            // Arrange
            var calculator = new OpenFrameCalculator();

            // Act
            var canHandle = calculator.CanHandle(FrameType.Open);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void OpenFrameCalculator_CalculateScore_ReturnsCorrectScore()
        {
            // Arrange
            var calculator = new OpenFrameCalculator();
            var frame = new Frame(FrameType.Open, 1, 0);
            var rolls = new List<Roll> { 3, 4 }; // Open frame: 3 + 4

            // Act
            var score = calculator.CalculateScore(frame, rolls);

            // Assert
            Assert.Equal(7, score); // 3 + 4
        }

        [Fact]
        public void OpenFrameCalculator_CalculateScore_WithWrongFrameType_ThrowsArgumentException()
        {
            // Arrange
            var calculator = new OpenFrameCalculator();
            var frame = new Frame(FrameType.Strike, 1, 0);
            var rolls = new List<Roll> { 10, 3, 4 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.CalculateScore(frame, rolls));
        }

        [Fact]
        public void OpenFrameCalculator_CalculateScore_WithIncompleteRolls_HandlesGracefully()
        {
            // Arrange
            var calculator = new OpenFrameCalculator();
            var frame = new Frame(FrameType.Open, 1, 0);
            var rolls = new List<Roll> { 3 }; // Only one roll available

            // Act
            var score = calculator.CalculateScore(frame, rolls);

            // Assert
            Assert.Equal(3, score); // Only first roll counted
        }
    }
}