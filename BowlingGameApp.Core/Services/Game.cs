using BowlingGameApp.Core.Interfaces;
using BowlingGameApp.Core.Models;
using BowlingGameApp.Core.Exceptions;

namespace BowlingGameApp.Core.Services
{
    /// <summary>
    /// Main game class that orchestrates a bowling game.
    /// Follows Single Responsibility Principle by delegating specific tasks to other services.
    /// Follows Dependency Inversion Principle by depending on abstractions.
    /// </summary>
    public class Game : IGame
    {
        private const int MaxRolls = 21;
        private readonly Roll[] _rolls = new Roll[MaxRolls];
        private readonly IScoreCalculator _scoreCalculator;
        private readonly IRollValidator _rollValidator;
        private int _currentRollIndex = 0;

        /// <summary>
        /// Initializes a new instance of the Game class.
        /// </summary>
        /// <param name="scoreCalculator">Service for calculating scores.</param>
        /// <param name="rollValidator">Service for validating rolls.</param>
        public Game(IScoreCalculator scoreCalculator, IRollValidator rollValidator)
        {
            _scoreCalculator = scoreCalculator ?? throw new ArgumentNullException(nameof(scoreCalculator));
            _rollValidator = rollValidator ?? throw new ArgumentNullException(nameof(rollValidator));
        }

        /// <inheritdoc />
        public void Roll(int pinsKnockedDown)
        {
            // Validate the roll using the validator service
            var currentRollsList = _rolls.Take(_currentRollIndex).Select(r => (int)r).ToList();

            if (!_rollValidator.IsValidRoll(pinsKnockedDown, currentRollsList, _currentRollIndex))
            {
                throw new InvalidRollException($"Invalid roll: {pinsKnockedDown} pins. Current game state doesn't allow this roll.");
            }

            // Record the roll
            _rolls[_currentRollIndex] = new Roll(pinsKnockedDown);
            _currentRollIndex++;
        }

        /// <inheritdoc />
        public int GetScore()
        {
            var rollsList = _rolls.Take(_currentRollIndex).ToList();
            return _scoreCalculator.CalculateTotalScore(rollsList);
        }

        /// <inheritdoc />
        public GameResult GetGameResult()
        {
            var rollsList = _rolls.Take(_currentRollIndex).ToList();
            return _scoreCalculator.GetGameResult(rollsList);
        }

        /// <inheritdoc />
        public bool IsGameComplete
        {
            get
            {
                // Game is complete when we have enough rolls for 10 frames
                // This is a simplified check - in a full implementation,
                // we'd need more sophisticated logic for the 10th frame
                return _currentRollIndex >= 18 ||
                       (_currentRollIndex >= 12 && AllFramesAreStrikes());
            }
        }

        /// <summary>
        /// Checks if all completed frames are strikes.
        /// </summary>
        private bool AllFramesAreStrikes()
        {
            for (int i = 0; i < Math.Min(_currentRollIndex, 10); i++)
            {
                if (_rolls[i].PinsKnockedDown != 10) return false;
            }
            return true;
        }
    }
}