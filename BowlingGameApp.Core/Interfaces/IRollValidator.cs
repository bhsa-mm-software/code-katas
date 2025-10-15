namespace BowlingGameApp.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for validating bowling rolls.
    /// Follows Single Responsibility Principle by focusing only on validation.
    /// </summary>
    public interface IRollValidator
    {
        /// <summary>
        /// Validates a roll given the current game state.
        /// </summary>
        /// <param name="pinsKnockedDown">Number of pins to knock down.</param>
        /// <param name="currentRolls">Current rolls in the game.</param>
        /// <param name="rollCount">Number of rolls completed so far.</param>
        /// <returns>True if the roll is valid.</returns>
        bool IsValidRoll(int pinsKnockedDown, IReadOnlyList<int> currentRolls, int rollCount);
    }
}