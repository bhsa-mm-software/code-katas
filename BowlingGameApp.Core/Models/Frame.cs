namespace BowlingGameApp.Core.Models
{
    /// <summary>
    /// Represents different types of frames in bowling.
    /// </summary>
    public enum FrameType
    {
        Open,   // Neither strike nor spare
        Spare,  // All 10 pins knocked down in 2 rolls
        Strike  // All 10 pins knocked down in 1 roll
    }

    /// <summary>
    /// Represents a frame in a bowling game.
    /// Contains information about the frame type and associated rolls.
    /// </summary>
    public class Frame
    {
        /// <summary>
        /// Gets the type of this frame (Open, Spare, or Strike).
        /// </summary>
        public FrameType Type { get; }

        /// <summary>
        /// Gets the frame number (1-10).
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// Gets the starting index of this frame's rolls in the rolls array.
        /// </summary>
        public int StartRollIndex { get; }

        /// <summary>
        /// Initializes a new instance of the Frame class.
        /// </summary>
        /// <param name="type">The type of frame.</param>
        /// <param name="number">The frame number (1-10).</param>
        /// <param name="startRollIndex">Starting index in the rolls array.</param>
        public Frame(FrameType type, int number, int startRollIndex)
        {
            Type = type;
            Number = number;
            StartRollIndex = startRollIndex;
        }

        /// <summary>
        /// Determines if this is the 10th (final) frame.
        /// </summary>
        public bool IsFinalFrame => Number == 10;
    }
}