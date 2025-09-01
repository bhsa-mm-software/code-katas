using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BowlingGameApp.Core.Interfaces;
using BowlingGameApp.Core.Services;
using BowlingGameApp.Core.Services.FrameCalculators;

namespace BowlingGameApp.Console
{
    /// <summary>
    /// Entry point for the bowling game console application.
    /// Demonstrates dependency injection and proper service registration.
    /// </summary>
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Configure dependency injection container
            var host = CreateHostBuilder(args).Build();

            // Run the bowling game demonstration
            var gameService = host.Services.GetRequiredService<IGame>();
            await RunBowlingGameDemo(gameService);
        }

        /// <summary>
        /// Creates and configures the host builder with dependency injection.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>Configured host builder.</returns>
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices);

        /// <summary>
        /// Configures dependency injection services.
        /// Demonstrates proper service registration following SOLID principles.
        /// </summary>
        /// <param name="services">Service collection to configure.</param>
        private static void ConfigureServices(IServiceCollection services)
        {
            // Register frame calculators (Strategy pattern implementations)
            services.AddTransient<IFrameCalculator, StrikeCalculator>();
            services.AddTransient<IFrameCalculator, SpareCalculator>();
            services.AddTransient<IFrameCalculator, OpenFrameCalculator>();

            // Register core services
            services.AddTransient<IRollValidator, RollValidator>();
            services.AddTransient<IScoreCalculator, ScoreCalculator>();
            services.AddTransient<IGame, Game>();
        }

        /// <summary>
        /// Demonstrates the bowling game with various scenarios.
        /// </summary>
        /// <param name="game">The game instance to use.</param>
        private static async Task RunBowlingGameDemo(IGame game)
        {
            System.Console.WriteLine("=== Bowling Game Demo ===\n");

            try
            {
                // Demonstrate a perfect game (all strikes)
                System.Console.WriteLine("Playing a perfect game (all strikes):");
                await PlayPerfectGame(game);

                // Create a new game for another demo
                var gameService = CreateNewGameInstance();
                System.Console.WriteLine("\nPlaying a mixed game:");
                await PlayMixedGame(gameService);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error during game: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new game instance (factory method pattern).
        /// </summary>
        /// <returns>A new game instance.</returns>
        private static IGame CreateNewGameInstance()
        {
            var frameCalculators = new List<IFrameCalculator>
            {
                new StrikeCalculator(),
                new SpareCalculator(),
                new OpenFrameCalculator()
            };

            var scoreCalculator = new ScoreCalculator(frameCalculators);
            var rollValidator = new RollValidator();

            return new Game(scoreCalculator, rollValidator);
        }

        /// <summary>
        /// Demonstrates a perfect game (300 points).
        /// </summary>
        /// <param name="game">The game instance.</param>
        private static async Task PlayPerfectGame(IGame game)
        {
            // 12 strikes for a perfect game
            for (int i = 0; i < 12; i++)
            {
                game.Roll(10); // Strike
                System.Console.WriteLine($"Roll {i + 1}: Strike! Current score: {game.GetScore()}");
                await Task.Delay(500); // Simulate time between rolls
            }

            var result = game.GetGameResult();
            System.Console.WriteLine($"\nFinal Result:");
            System.Console.WriteLine($"Total Score: {result.TotalScore}");
            System.Console.WriteLine($"Strikes: {result.StrikesCount}");
            System.Console.WriteLine($"Perfect Game: {result.IsPerfectGame}");
        }

        /// <summary>
        /// Demonstrates a mixed game with strikes, spares, and open frames.
        /// </summary>
        /// <param name="game">The game instance.</param>
        private static async Task PlayMixedGame(IGame game)
        {
            var rolls = new int[] {
                10,      // Frame 1: Strike
                7, 3,    // Frame 2: Spare
                9, 0,    // Frame 3: Open
                10,      // Frame 4: Strike
                0, 8,    // Frame 5: Open
                8, 2,    // Frame 6: Spare
                0, 6,    // Frame 7: Open
                10,      // Frame 8: Strike
                10,      // Frame 9: Strike
                10, 8, 1 // Frame 10: Strike + 8 + 1
            };

            for (int i = 0; i < rolls.Length; i++)
            {
                game.Roll(rolls[i]);
                System.Console.WriteLine($"Roll {i + 1}: {rolls[i]} pins. Current score: {game.GetScore()}");
                await Task.Delay(300);
            }

            var result = game.GetGameResult();
            System.Console.WriteLine($"\nFinal Result:");
            System.Console.WriteLine($"Total Score: {result.TotalScore}");
            System.Console.WriteLine($"Strikes: {result.StrikesCount}");
            System.Console.WriteLine($"Spares: {result.SparesCount}");
            System.Console.WriteLine($"Perfect Game: {result.IsPerfectGame}");
        }
    }
}