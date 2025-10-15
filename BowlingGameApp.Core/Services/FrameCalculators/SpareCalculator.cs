using BowlingGameApp.Core.Interfaces;
using BowlingGameApp.Core.Models;

namespace BowlingGameApp.Core.Services.FrameCalculators
{
    /// <summary>
    /// Calculator for spare frames.
    /// Follows Single Responsibility Principle by handling only spare calculations.
    /// </summary>
    public class SpareCalculator : IFrameCalculator
    {
        /// <inheritdoc />
        public bool CanHandle(FrameType frameType) => frameType == FrameType.Spare;

        /// <inheritdoc />
        public int CalculateScore(Frame frame, IReadOnlyList<Roll> rolls)
        {
            if (!CanHandle(frame.Type))
                throw new ArgumentException($"Cannot handle frame type: {frame.Type}");

            const int spareScore = 10;

            // For spares, we get the next roll as bonus
            var bonusScore = GetSpareBonus(frame, rolls);

            return spareScore + bonusScore;
        }

        /// <summary>
        /// Calculates the bonus score for a spare (next roll).
        /// </summary>
        /// <param name="frame">The spare frame.</param>
        /// <param name="rolls">All rolls in the game.</param>
        /// <returns>The bonus score.</returns>
        private int GetSpareBonus(Frame frame, IReadOnlyList<Roll> rolls)
        {
            var rollIndex = frame.StartRollIndex;

            // Ensure we have enough rolls for bonus calculation
            if (rollIndex + 2 >= rolls.Count)
                return 0; // No bonus if not enough rolls

            return rolls[rollIndex + 2].PinsKnockedDown;
        }
    }
}