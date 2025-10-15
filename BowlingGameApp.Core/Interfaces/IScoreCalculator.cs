using BowlingGameApp.Core.Models;

namespace BowlingGameApp.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for calculating total game scores.
    /// </summary>
    public interface IScoreCalculator
    {
        /// <summary>
        /// Calculates the total score for all completed frames.
        /// </summary>
        /// <param name="rolls">All rolls in the game.</param>
        /// <returns>The total score.</returns>
        int CalculateTotalScore(IReadOnlyList<Roll> rolls);

        /// <summary>
        /// Gets the complete game result with statistics.
        /// </summary>
        /// <param name="rolls">All rolls in the game.</param>
        /// <returns>Complete game result.</returns>
        GameResult GetGameResult(IReadOnlyList<Roll> rolls);
    }
}