using BowlingGameApp.Core.Models;

namespace BowlingGameApp.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for calculating frame scores.
    /// Follows the Strategy pattern for different frame types.
    /// </summary>
    public interface IFrameCalculator
    {
        /// <summary>
        /// Determines if this calculator can handle the specified frame type.
        /// </summary>
        /// <param name="frameType">The type of frame to check.</param>
        /// <returns>True if this calculator can handle the frame type.</returns>
        bool CanHandle(FrameType frameType);

        /// <summary>
        /// Calculates the score for a frame.
        /// </summary>
        /// <param name="frame">The frame to calculate.</param>
        /// <param name="rolls">All rolls in the game.</param>
        /// <returns>The score for this frame including bonuses.</returns>
        int CalculateScore(Frame frame, IReadOnlyList<Roll> rolls);
    }
}