using BowlingGameApp.Core.Models;

namespace BowlingGameApp.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a bowling game.
    /// Follows the Interface Segregation Principle by providing a focused interface.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Records a roll with the specified number of pins knocked down.
        /// </summary>
        /// <param name="pinsKnockedDown">Number of pins knocked down (0-10).</param>
        /// <exception cref="InvalidRollException">Thrown when the roll is invalid.</exception>
        void Roll(int pinsKnockedDown);

        /// <summary>
        /// Calculates and returns the current score of the game.
        /// </summary>
        /// <returns>The current total score.</returns>
        int GetScore();

        /// <summary>
        /// Gets the complete game result including statistics.
        /// </summary>
        /// <returns>A GameResult object containing score and statistics.</returns>
        GameResult GetGameResult();

        /// <summary>
        /// Gets whether the game is complete (all frames played).
        /// </summary>
        bool IsGameComplete { get; }
    }
}