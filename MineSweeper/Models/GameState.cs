namespace MineSweeper.Models;

public enum GameStatus
{
    NotStarted,
    InProgress,
    Won,
    Lost
}

public class GameState
{
    public Cell[,] Board { get; set; }
    public int Rows { get; set; }
    public int Columns { get; set; }
    public int TotalMines { get; set; }
    public int FlaggedCells { get; set; }
    public int RevealedCells { get; set; }
    public GameStatus Status { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public GameDifficulty Difficulty { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int RemainingMines => TotalMines - FlaggedCells;
    public TimeSpan ElapsedTime => Status == GameStatus.InProgress && StartTime.HasValue 
        ? DateTime.Now - StartTime.Value 
        : EndTime.HasValue && StartTime.HasValue 
            ? EndTime.Value - StartTime.Value 
            : TimeSpan.Zero;
    
    public GameState(GameDifficulty difficulty)
    {
        Difficulty = difficulty;
        var (rows, columns, mines) = DifficultySettings.GetSettings(difficulty);
        Rows = rows;
        Columns = columns;
        TotalMines = mines;
        Board = new Cell[rows, columns];
        Status = GameStatus.NotStarted;
        CreatedAt = DateTime.Now;
        InitializeBoard();
    }
    
    private void InitializeBoard()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                Board[row, col] = new Cell(row, col);
            }
        }
    }
}