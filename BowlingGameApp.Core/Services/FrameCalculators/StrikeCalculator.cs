using BowlingGameApp.Core.Interfaces;
using BowlingGameApp.Core.Models;

namespace BowlingGameApp.Core.Services.FrameCalculators
{
    /// <summary>
    /// Calculator for strike frames.
    /// Follows Single Responsibility Principle by handling only strike calculations.
    /// </summary>
    public class StrikeCalculator : IFrameCalculator
    {
        /// <inheritdoc />
        public bool CanHandle(FrameType frameType) => frameType == FrameType.Strike;

        /// <inheritdoc />
        public int CalculateScore(Frame frame, IReadOnlyList<Roll> rolls)
        {
            if (!CanHandle(frame.Type))
                throw new ArgumentException($"Cannot handle frame type: {frame.Type}");

            const int strikeScore = 10;

            // For strikes, we get the next two rolls as bonus
            var bonusScore = GetStrikeBonus(frame, rolls);

            return strikeScore + bonusScore;
        }

        /// <summary>
        /// Calculates the bonus score for a strike (next two rolls).
        /// </summary>
        /// <param name="frame">The strike frame.</param>
        /// <param name="rolls">All rolls in the game.</param>
        /// <returns>The bonus score.</returns>
        private int GetStrikeBonus(Frame frame, IReadOnlyList<Roll> rolls)
        {
            var rollIndex = frame.StartRollIndex;

            // Ensure we have enough rolls for bonus calculation
            if (rollIndex + 2 >= rolls.Count)
                return 0; // No bonus if not enough rolls

            return rolls[rollIndex + 1].PinsKnockedDown + rolls[rollIndex + 2].PinsKnockedDown;
        }
    }
}