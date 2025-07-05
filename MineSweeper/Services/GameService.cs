using MineSweeper.Models;

namespace MineSweeper.Services;

public class GameService
{
    private readonly Random _random = new();
    public GameState? CurrentGame { get; private set; }
    
    public event Action? OnGameStateChanged;
    
    public void NewGame(GameDifficulty difficulty)
    {
        CurrentGame = new GameState(difficulty);
        OnGameStateChanged?.Invoke();
    }
    
    public void LoadGame(SavedGameState savedState)
    {
        CurrentGame = new GameState(savedState.Difficulty)
        {
            Status = savedState.Status,
            FlaggedCells = savedState.FlaggedCells,
            RevealedCells = savedState.RevealedCells,
            StartTime = savedState.StartTime,
            EndTime = savedState.EndTime
        };
        
        // Restore board state
        foreach (var cellData in savedState.BoardData)
        {
            var cell = CurrentGame.Board[cellData.Row, cellData.Col];
            cell.IsMine = cellData.IsMine;
            cell.IsRevealed = cellData.IsRevealed;
            cell.IsFlagged = cellData.IsFlagged;
            cell.AdjacentMines = cellData.AdjacentMines;
        }
        
        OnGameStateChanged?.Invoke();
    }
    
    public void StartGame(int firstRow, int firstCol)
    {
        if (CurrentGame == null || CurrentGame.Status != GameStatus.NotStarted)
            return;
            
        PlaceMines(firstRow, firstCol);
        CalculateAdjacentMines();
        CurrentGame.Status = GameStatus.InProgress;
        CurrentGame.StartTime = DateTime.Now;
        RevealCell(firstRow, firstCol);
    }
    
    private void PlaceMines(int safeRow, int safeCol)
    {
        if (CurrentGame == null) return;
        
        int minesPlaced = 0;
        while (minesPlaced < CurrentGame.TotalMines)
        {
            int row = _random.Next(CurrentGame.Rows);
            int col = _random.Next(CurrentGame.Columns);
            
            if (CurrentGame.Board[row, col].IsMine || (row == safeRow && col == safeCol))
                continue;
                
            CurrentGame.Board[row, col].IsMine = true;
            minesPlaced++;
        }
    }
    
    private void CalculateAdjacentMines()
    {
        if (CurrentGame == null) return;
        
        for (int row = 0; row < CurrentGame.Rows; row++)
        {
            for (int col = 0; col < CurrentGame.Columns; col++)
            {
                if (!CurrentGame.Board[row, col].IsMine)
                {
                    CurrentGame.Board[row, col].AdjacentMines = CountAdjacentMines(row, col);
                }
            }
        }
    }
    
    private int CountAdjacentMines(int row, int col)
    {
        if (CurrentGame == null) return 0;
        
        int count = 0;
        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0) continue;
                
                int newRow = row + dr;
                int newCol = col + dc;
                
                if (IsValidCell(newRow, newCol) && CurrentGame.Board[newRow, newCol].IsMine)
                {
                    count++;
                }
            }
        }
        return count;
    }
    
    private bool IsValidCell(int row, int col)
    {
        return CurrentGame != null && 
               row >= 0 && row < CurrentGame.Rows && 
               col >= 0 && col < CurrentGame.Columns;
    }
    
    public void RevealCell(int row, int col)
    {
        if (CurrentGame == null) return;
        
        if (CurrentGame.Status == GameStatus.NotStarted)
        {
            StartGame(row, col);
            return;
        }
        
        if (CurrentGame.Status != GameStatus.InProgress) return;
        
        var cell = CurrentGame.Board[row, col];
        
        if (cell.IsRevealed || cell.IsFlagged) return;
        
        cell.IsRevealed = true;
        CurrentGame.RevealedCells++;
        
        if (cell.IsMine)
        {
            CurrentGame.Status = GameStatus.Lost;
            CurrentGame.EndTime = DateTime.Now;
            RevealAllMines();
        }
        else
        {
            if (cell.AdjacentMines == 0)
            {
                RevealAdjacentCells(row, col);
            }
            
            CheckWinCondition();
        }
        
        OnGameStateChanged?.Invoke();
    }
    
    private void RevealAdjacentCells(int row, int col)
    {
        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0) continue;
                
                int newRow = row + dr;
                int newCol = col + dc;
                
                if (IsValidCell(newRow, newCol))
                {
                    RevealCell(newRow, newCol);
                }
            }
        }
    }
    
    private void RevealAllMines()
    {
        if (CurrentGame == null) return;
        
        for (int row = 0; row < CurrentGame.Rows; row++)
        {
            for (int col = 0; col < CurrentGame.Columns; col++)
            {
                if (CurrentGame.Board[row, col].IsMine)
                {
                    CurrentGame.Board[row, col].IsRevealed = true;
                }
            }
        }
    }
    
    public void ToggleFlag(int row, int col)
    {
        if (CurrentGame == null || CurrentGame.Status != GameStatus.InProgress) return;
        
        var cell = CurrentGame.Board[row, col];
        
        if (cell.IsRevealed) return;
        
        if (cell.IsFlagged)
        {
            cell.IsFlagged = false;
            CurrentGame.FlaggedCells--;
        }
        else if (CurrentGame.FlaggedCells < CurrentGame.TotalMines)
        {
            cell.IsFlagged = true;
            CurrentGame.FlaggedCells++;
        }
        
        OnGameStateChanged?.Invoke();
    }
    
    private void CheckWinCondition()
    {
        if (CurrentGame == null) return;
        
        int totalCells = CurrentGame.Rows * CurrentGame.Columns;
        int cellsToReveal = totalCells - CurrentGame.TotalMines;
        
        if (CurrentGame.RevealedCells == cellsToReveal)
        {
            CurrentGame.Status = GameStatus.Won;
            CurrentGame.EndTime = DateTime.Now;
            FlagAllMines();
        }
    }
    
    private void FlagAllMines()
    {
        if (CurrentGame == null) return;
        
        for (int row = 0; row < CurrentGame.Rows; row++)
        {
            for (int col = 0; col < CurrentGame.Columns; col++)
            {
                if (CurrentGame.Board[row, col].IsMine && !CurrentGame.Board[row, col].IsFlagged)
                {
                    CurrentGame.Board[row, col].IsFlagged = true;
                    CurrentGame.FlaggedCells++;
                }
            }
        }
    }
    
    public void RevealAdjacentIfSafe(int row, int col)
    {
        if (CurrentGame == null || CurrentGame.Status != GameStatus.InProgress) return;
        
        var cell = CurrentGame.Board[row, col];
        if (!cell.IsRevealed || cell.AdjacentMines == 0) return;
        
        int flaggedCount = 0;
        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0) continue;
                
                int newRow = row + dr;
                int newCol = col + dc;
                
                if (IsValidCell(newRow, newCol) && CurrentGame.Board[newRow, newCol].IsFlagged)
                {
                    flaggedCount++;
                }
            }
        }
        
        if (flaggedCount == cell.AdjacentMines)
        {
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0) continue;
                    
                    int newRow = row + dr;
                    int newCol = col + dc;
                    
                    if (IsValidCell(newRow, newCol))
                    {
                        var adjacentCell = CurrentGame.Board[newRow, newCol];
                        if (!adjacentCell.IsRevealed && !adjacentCell.IsFlagged)
                        {
                            RevealCell(newRow, newCol);
                        }
                    }
                }
            }
        }
    }
}