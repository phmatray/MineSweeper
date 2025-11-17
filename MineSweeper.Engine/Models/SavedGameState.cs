namespace MineSweeper.Engine.Models;

public class SavedGameState
{
    public GameDifficulty Difficulty { get; set; }
    public GameStatus Status { get; set; }
    public int Rows { get; set; }
    public int Columns { get; set; }
    public int TotalMines { get; set; }
    public int FlaggedCells { get; set; }
    public int RevealedCells { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CellData> BoardData { get; set; } = new();
}

public class CellData
{
    public int Row { get; set; }
    public int Col { get; set; }
    public bool IsMine { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsFlagged { get; set; }
    public int AdjacentMines { get; set; }
}