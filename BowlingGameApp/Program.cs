namespace BowlingGameApp;

internal class Program
{
    private static void Main()
    {
        var game = new Game();
        Console.WriteLine("Welcome to the Bowling Game!");
        Console.WriteLine("Enter the number of pins knocked down for each roll.\n");

        var frame = 1;
        var rollIndex = 1;

        while (frame <= 10)
        {
            Console.Write($"Frame {frame}, Roll {rollIndex}: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out var pins) || pins is < 0 or > 10)
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 10.");
                continue;
            }

            try
            {
                game.Roll(pins);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                continue;
            }

            // Handle frames 1-9
            if (frame < 10)
            {
                if (pins == 10 && rollIndex == 1) // Strike
                {
                    Console.WriteLine("Strike!");
                    frame++;
                }
                else if (rollIndex == 2) // Completed second roll
                {
                    frame++;
                    rollIndex = 1;
                }
                else
                {
                    rollIndex = 2;
                }
            }
            else // 10th frame logic
            {
                // Max 3 rolls in 10th frame if strike or spare
                if (rollIndex == 1)
                {
                    rollIndex = 2;
                }
                else if (rollIndex == 2)
                {
                    var isStrikeOrSpare = (pins == 10 || game.Score() >= 100); // crude check, can refine
                    rollIndex = isStrikeOrSpare ? 3 : 0;
                }
                else
                {
                    break; // 3 rolls done
                }

                if (rollIndex == 0)
                    break;
            }
        }

        Console.WriteLine("\nGame completed!");
        Console.WriteLine($"Final Score: {game.Score()}");
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}
