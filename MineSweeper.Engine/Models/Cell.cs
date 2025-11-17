namespace MineSweeper.Engine.Models;

public class Cell
{
    public int Row { get; set; }
    public int Column { get; set; }
    public bool IsMine { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsFlagged { get; set; }
    public int AdjacentMines { get; set; }

    public Cell(int row, int column)
    {
        Row = row;
        Column = column;
    }
}