using BowlingGameApp.Core.Interfaces;
using BowlingGameApp.Core.Models;

namespace BowlingGameApp.Core.Services.FrameCalculators
{
    /// <summary>
    /// Calculator for open frames (neither strike nor spare).
    /// Follows Single Responsibility Principle by handling only open frame calculations.
    /// </summary>
    public class OpenFrameCalculator : IFrameCalculator
    {
        /// <inheritdoc />
        public bool CanHandle(FrameType frameType) => frameType == FrameType.Open;

        /// <inheritdoc />
        public int CalculateScore(Frame frame, IReadOnlyList<Roll> rolls)
        {
            if (!CanHandle(frame.Type))
                throw new ArgumentException($"Cannot handle frame type: {frame.Type}");

            var rollIndex = frame.StartRollIndex;

            // For open frames, just sum the two rolls with no bonus
            var firstRoll = rollIndex < rolls.Count ? rolls[rollIndex].PinsKnockedDown : 0;
            var secondRoll = rollIndex + 1 < rolls.Count ? rolls[rollIndex + 1].PinsKnockedDown : 0;

            return firstRoll + secondRoll;
        }
    }
}