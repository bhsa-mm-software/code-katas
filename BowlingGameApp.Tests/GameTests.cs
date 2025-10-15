namespace BowlingGameApp.Tests;

public class GameTests
{
    private readonly Game _game;

    public GameTests()
    {
        _game = new Game();
    }

    private void RollMany(int rolls, int pins)
    {
        for (var i = 0; i < rolls; i++)
        {
            _game.Roll(pins);
        }
    }

    [Fact]
    public void GutterGameScoresZero()
    {
        RollMany(20, 0);
        Assert.Equal(0, _game.Score());
    }

    [Fact]
    public void AllOnesScores20()
    {
        RollMany(20, 1);
        Assert.Equal(20, _game.Score());
    }

    [Fact]
    public void OneSpareAddsNextRollBonus()
    {
        _game.Roll(5);
        _game.Roll(5); // spare
        _game.Roll(3);
        RollMany(17, 0);
        Assert.Equal(16, _game.Score());
    }

    [Fact]
    public void OneStrikeAddsNextTwoRollsBonus()
    {
        _game.Roll(10); // strike
        _game.Roll(3);
        _game.Roll(4);
        RollMany(16, 0);
        Assert.Equal(24, _game.Score());
    }

    [Fact]
    public void PerfectGameScores300()
    {
        RollMany(12, 10);
        Assert.Equal(300, _game.Score());
    }
}