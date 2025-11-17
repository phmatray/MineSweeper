namespace MineSweeper.Engine.Models;

public enum GameDifficulty
{
    Beginner,
    Intermediate,
    Expert
}

public static class DifficultySettings
{
    public static (int rows, int columns, int mines) GetSettings(GameDifficulty difficulty)
    {
        return difficulty switch
        {
            GameDifficulty.Beginner => (9, 9, 10),
            GameDifficulty.Intermediate => (16, 16, 40),
            GameDifficulty.Expert => (16, 30, 99),
            _ => (9, 9, 10)
        };
    }
}