using BowlingGameApp.Core.Interfaces;

namespace BowlingGameApp.Core.Services
{
    /// <summary>
    /// Service for validating bowling rolls.
    /// Follows Single Responsibility Principle by focusing only on validation logic.
    /// </summary>
    public class RollValidator : IRollValidator
    {
        /// <inheritdoc />
        public bool IsValidRoll(int pinsKnockedDown, IReadOnlyList<int> currentRolls, int rollCount)
        {
            // Basic validation: pins must be between 0 and 10
            if (pinsKnockedDown is < 0 or > 10)
                return false;

            // Always allow the first roll
            if (rollCount == 0)
                return true;

            // Determine which frame we're in and validate accordingly
            var frameInfo = GetCurrentFrameInfo(currentRolls, rollCount);

            if (frameInfo.FrameNumber <= 9)
            {
                return ValidateRegularFrameRoll(pinsKnockedDown, currentRolls, rollCount, frameInfo);
            }
            else
            {
                return ValidateRollInTenthFrame(pinsKnockedDown, currentRolls, rollCount, frameInfo);
            }
        }

        /// <summary>
        /// Gets information about the current frame being played.
        /// </summary>
        /// <param name="currentRolls">All current rolls.</param>
        /// <param name="rollCount">Current roll count.</param>
        /// <returns>Frame information.</returns>
        private (int FrameNumber, int RollInFrame, int FrameStartIndex) GetCurrentFrameInfo(IReadOnlyList<int> currentRolls, int rollCount)
        {
            var frameNumber = 1;
            var rollIndex = 0;

            // Count through completed frames
            while (frameNumber <= 9 && rollIndex < rollCount)
            {
                if (rollIndex < currentRolls.Count && currentRolls[rollIndex] == 10)
                {
                    // Strike - move to next frame
                    rollIndex++;
                    if (rollIndex == rollCount)
                        return (frameNumber, 1, rollIndex - 1); // Currently rolling in this frame
                    frameNumber++;
                }
                else
                {
                    // Not a strike - check if we have both rolls for this frame
                    if (rollIndex + 1 < rollCount)
                    {
                        // Both rolls completed, move to next frame
                        rollIndex += 2;
                        frameNumber++;
                    }
                    else if (rollIndex + 1 == rollCount)
                    {
                        // Second roll of current frame
                        return (frameNumber, 2, rollIndex);
                    }
                    else
                    {
                        // First roll of current frame
                        return (frameNumber, 1, rollIndex);
                    }
                }
            }

            // We're in the 10th frame
            var rollInTenthFrame = rollCount - rollIndex + 1;
            return (10, rollInTenthFrame, rollIndex);
        }

        /// <summary>
        /// Validates a roll in frames 1-9.
        /// </summary>
        /// <param name="pinsKnockedDown">Pins to knock down.</param>
        /// <param name="currentRolls">All current rolls.</param>
        /// <param name="rollCount">Current roll count.</param>
        /// <param name="frameInfo">Current frame information.</param>
        /// <returns>True if the roll is valid.</returns>
        private bool ValidateRegularFrameRoll(int pinsKnockedDown, IReadOnlyList<int> currentRolls,
            int rollCount, (int FrameNumber, int RollInFrame, int FrameStartIndex) frameInfo)
        {
            if (frameInfo.RollInFrame == 1)
            {
                // First roll of frame is always valid (0-10)
                return true;
            }
            else if (frameInfo.RollInFrame == 2)
            {
                // Second roll: ensure total doesn't exceed 10
                var firstRoll = currentRolls[rollCount - 1];
                return firstRoll + pinsKnockedDown <= 10;
            }

            return false; // Shouldn't happen in regular frames
        }

        /// <summary>
        /// Validates a roll specifically in the 10th frame.
        /// The 10th frame has special rules for strikes and spares.
        /// </summary>
        /// <param name="pinsKnockedDown">Pins to knock down.</param>
        /// <param name="currentRolls">All current rolls.</param>
        /// <param name="rollCount">Current roll count.</param>
        /// <param name="frameInfo">Current frame information.</param>
        /// <returns>True if the roll is valid in the 10th frame.</returns>
        private bool ValidateRollInTenthFrame(int pinsKnockedDown, IReadOnlyList<int> currentRolls,
            int rollCount, (int FrameNumber, int RollInFrame, int FrameStartIndex) frameInfo)
        {
            var tenthFrameStartIndex = frameInfo.FrameStartIndex;
            var rollInTenthFrame = frameInfo.RollInFrame;

            if (rollInTenthFrame == 1)
            {
                // First roll in 10th frame is always valid (0-10)
                return true;
            }
            else if (rollInTenthFrame == 2)
            {
                // Second roll in 10th frame
                var firstRoll = currentRolls[tenthFrameStartIndex];

                if (firstRoll == 10)
                {
                    // First roll was a strike, so second roll is independent (0-10)
                    return true;
                }
                else
                {
                    // First roll was not a strike, so total can't exceed 10
                    return firstRoll + pinsKnockedDown <= 10;
                }
            }
            else if (rollInTenthFrame == 3)
            {
                // Third roll in 10th frame (bonus roll)
                var firstRoll = currentRolls[tenthFrameStartIndex];
                var secondRoll = currentRolls[tenthFrameStartIndex + 1];

                // Third roll is only allowed if first was strike OR first two made a spare
                if (firstRoll == 10 || firstRoll + secondRoll == 10)
                {
                    // If second roll was a strike, third roll is independent
                    if (secondRoll == 10)
                        return true;

                    // If second roll wasn't a strike, we need to consider the context
                    if (firstRoll == 10)
                    {
                        // First was strike, second wasn't strike
                        // Third roll + second roll can't exceed 10 (unless second was 10)
                        return secondRoll + pinsKnockedDown <= 10;
                    }
                    else
                    {
                        // First two made a spare, third roll is independent
                        return true;
                    }
                }

                return false; // No third roll allowed
            }

            return false; // More than 3 rolls not allowed
        }
    }
}