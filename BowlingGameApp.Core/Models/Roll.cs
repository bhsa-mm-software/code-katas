namespace BowlingGameApp.Core.Models
{
    /// <summary>
    /// Represents a single roll in a bowling game.
    /// Encapsulates the number of pins knocked down with validation.
    /// </summary>
    public readonly record struct Roll
    {
        /// <summary>
        /// Gets the number of pins knocked down in this roll.
        /// </summary>
        public int PinsKnockedDown { get; }

        /// <summary>
        /// Initializes a new instance of the Roll struct.
        /// </summary>
        /// <param name="pinsKnockedDown">Number of pins knocked down (0-10).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when pins is not between 0 and 10.</exception>
        public Roll(int pinsKnockedDown)
        {
            if (pinsKnockedDown is < 0 or > 10)
                throw new ArgumentOutOfRangeException(nameof(pinsKnockedDown),
                    "Pins knocked down must be between 0 and 10.");

            PinsKnockedDown = pinsKnockedDown;
        }

        /// <summary>
        /// Implicitly converts an integer to a Roll.
        /// </summary>
        public static implicit operator Roll(int pins) => new(pins);

        /// <summary>
        /// Implicitly converts a Roll to an integer.
        /// </summary>
        public static implicit operator int(Roll roll) => roll.PinsKnockedDown;
    }
}
