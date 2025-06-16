namespace BowlingGameApp;

public class Game
{
    // A bowling game can have at most 21 rolls (including bonus rolls in the 10th frame)
    private readonly int[] _rolls = new int[21];
    private int _currentRoll = 0;

    /// Records the number of pins knocked down in a single roll.
    public void Roll(int pins)
    {
        if (pins is < 0 or > 10)
            throw new ArgumentOutOfRangeException(nameof(pins), "Pins must be between 0 and 10.");

        _rolls[_currentRoll++] = pins;
    }

    /// Calculates and returns the total score for the game after all rolls are completed.
    public int Score()
    {
        var score = 0;
        var rollIndex = 0;

        for (var frame = 0; frame < 10; frame++)
        {
            if (IsStrike(rollIndex)) // Strike
            {
                score += 10 + StrikeBonus(rollIndex);
                rollIndex += 1;
            }
            else if (IsSpare(rollIndex)) // Spare
            {
                score += 10 + SpareBonus(rollIndex);
                rollIndex += 2;
            }
            else // Open frame
            {
                score += SumOfBallsInFrame(rollIndex);
                rollIndex += 2;
            }
        }

        return score;
    }

    /// Checks if the roll at the given index is a strike.
    private bool IsStrike(int rollIndex) => _rolls[rollIndex] == 10;

    /// Checks if the frame starting at the given index is a spare.
    private bool IsSpare(int rollIndex) =>
        _rolls[rollIndex] + _rolls[rollIndex + 1] == 10;

    /// Calculates the bonus for a strike (next two rolls).
    private int StrikeBonus(int rollIndex) =>
        _rolls[rollIndex + 1] + _rolls[rollIndex + 2];

    /// Calculates the bonus for a spare (next roll).
    private int SpareBonus(int rollIndex) =>
        _rolls[rollIndex + 2];

    /// Returns the sum of two rolls in a frame.
    private int SumOfBallsInFrame(int rollIndex) =>
        _rolls[rollIndex] + _rolls[rollIndex + 1];
}