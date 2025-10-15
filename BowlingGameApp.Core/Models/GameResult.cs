namespace BowlingGameApp.Core.Models
{
    /// <summary>
    /// Represents the final result of a bowling game.
    /// Contains the total score and additional game statistics.
    /// </summary>
    public class GameResult
    {
        /// <summary>
        /// Gets the total score for the game.
        /// </summary>
        public int TotalScore { get; }

        /// <summary>
        /// Gets the number of strikes in the game.
        /// </summary>
        public int StrikesCount { get; }

        /// <summary>
        /// Gets the number of spares in the game.
        /// </summary>
        public int SparesCount { get; }

        /// <summary>
        /// Gets whether this is a perfect game (score of 300).
        /// </summary>
        public bool IsPerfectGame => TotalScore == 300;

        /// <summary>
        /// Initializes a new instance of the GameResult class.
        /// </summary>
        /// <param name="totalScore">The total score.</param>
        /// <param name="strikesCount">Number of strikes.</param>
        /// <param name="sparesCount">Number of spares.</param>
        public GameResult(int totalScore, int strikesCount, int sparesCount)
        {
            TotalScore = totalScore;
            StrikesCount = strikesCount;
            SparesCount = sparesCount;
        }
    }
}