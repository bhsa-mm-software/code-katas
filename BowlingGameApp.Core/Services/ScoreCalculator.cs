using BowlingGameApp.Core.Interfaces;
using BowlingGameApp.Core.Models;

namespace BowlingGameApp.Core.Services
{
    /// <summary>
    /// Service responsible for calculating bowling game scores.
    /// Follows Single Responsibility Principle and Open/Closed Principle.
    /// </summary>
    public class ScoreCalculator : IScoreCalculator
    {
        private readonly IEnumerable<IFrameCalculator> _frameCalculators;

        /// <summary>
        /// Initializes a new instance of the ScoreCalculator class.
        /// </summary>
        /// <param name="frameCalculators">Collection of frame calculators for different frame types.</param>
        public ScoreCalculator(IEnumerable<IFrameCalculator> frameCalculators)
        {
            _frameCalculators = frameCalculators ?? throw new ArgumentNullException(nameof(frameCalculators));
        }

        /// <inheritdoc />
        public int CalculateTotalScore(IReadOnlyList<Roll> rolls)
        {
            var frames = ParseFrames(rolls);
            var totalScore = 0;

            foreach (var frame in frames)
            {
                var calculator = GetFrameCalculator(frame.Type);
                totalScore += calculator.CalculateScore(frame, rolls);
            }

            return totalScore;
        }

        /// <inheritdoc />
        public GameResult GetGameResult(IReadOnlyList<Roll> rolls)
        {
            var frames = ParseFrames(rolls);
            var totalScore = CalculateTotalScore(rolls);

            var strikesCount = frames.Count(f => f.Type == FrameType.Strike);
            var sparesCount = frames.Count(f => f.Type == FrameType.Spare);

            return new GameResult(totalScore, strikesCount, sparesCount);
        }

        /// <summary>
        /// Parses rolls into frame objects.
        /// </summary>
        /// <param name="rolls">All rolls in the game.</param>
        /// <returns>Collection of parsed frames.</returns>
        private List<Frame> ParseFrames(IReadOnlyList<Roll> rolls)
        {
            var frames = new List<Frame>();
            var rollIndex = 0;

            // Parse first 9 frames
            for (var frameNumber = 1; frameNumber <= 9 && rollIndex < rolls.Count; frameNumber++)
            {
                var frameType = DetermineFrameType(rolls, rollIndex);
                frames.Add(new Frame(frameType, frameNumber, rollIndex));

                // Move to next frame
                rollIndex += frameType == FrameType.Strike ? 1 : 2;
            }

            // Parse 10th frame (special handling needed)
            if (rollIndex < rolls.Count)
            {
                var tenthFrameType = DetermineFrameType(rolls, rollIndex);
                frames.Add(new Frame(tenthFrameType, 10, rollIndex));
            }

            return frames;
        }

        /// <summary>
        /// Determines the type of frame based on the rolls.
        /// </summary>
        /// <param name="rolls">All rolls in the game.</param>
        /// <param name="startIndex">Starting index for this frame.</param>
        /// <returns>The frame type.</returns>
        private FrameType DetermineFrameType(IReadOnlyList<Roll> rolls, int startIndex)
        {
            if (startIndex >= rolls.Count) return FrameType.Open;

            // Check for strike
            if (rolls[startIndex].PinsKnockedDown == 10)
                return FrameType.Strike;

            // Check for spare
            if (startIndex + 1 < rolls.Count &&
                rolls[startIndex].PinsKnockedDown + rolls[startIndex + 1].PinsKnockedDown == 10)
                return FrameType.Spare;

            return FrameType.Open;
        }

        /// <summary>
        /// Gets the appropriate frame calculator for the given frame type.
        /// </summary>
        /// <param name="frameType">The type of frame.</param>
        /// <returns>The frame calculator.</returns>
        /// <exception cref="NotSupportedException">Thrown when no calculator is found.</exception>
        private IFrameCalculator GetFrameCalculator(FrameType frameType)
        {
            var calculator = _frameCalculators.FirstOrDefault(c => c.CanHandle(frameType));
            return calculator ?? throw new NotSupportedException($"No calculator found for frame type: {frameType}");
        }
    }
}