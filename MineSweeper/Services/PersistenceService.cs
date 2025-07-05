using System.Text.Json;
using Microsoft.JSInterop;
using MineSweeper.Models;

namespace MineSweeper.Services;

public class PersistenceService
{
    private readonly IJSRuntime _jsRuntime;
    private const string GameStateKey = "minesweeper_current_game";
    private const string SoundSettingsKey = "minesweeper_sound_settings";
    
    public PersistenceService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    
    public async Task SaveGameStateAsync(GameState? gameState)
    {
        try
        {
            if (gameState == null || gameState.Status == GameStatus.NotStarted)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", GameStateKey);
                return;
            }
            
            var savedState = new SavedGameState
            {
                Difficulty = gameState.Difficulty,
                Status = gameState.Status,
                Rows = gameState.Rows,
                Columns = gameState.Columns,
                TotalMines = gameState.TotalMines,
                FlaggedCells = gameState.FlaggedCells,
                RevealedCells = gameState.RevealedCells,
                StartTime = gameState.StartTime,
                EndTime = gameState.EndTime,
                CreatedAt = gameState.CreatedAt,
                BoardData = SerializeBoard(gameState.Board)
            };
            
            var json = JsonSerializer.Serialize(savedState);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", GameStateKey, json);
        }
        catch
        {
            // Handle gracefully if localStorage is not available
        }
    }
    
    public async Task<SavedGameState?> LoadGameStateAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", GameStateKey);
            if (string.IsNullOrEmpty(json))
                return null;
                
            return JsonSerializer.Deserialize<SavedGameState>(json);
        }
        catch
        {
            return null;
        }
    }
    
    public async Task ClearGameStateAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", GameStateKey);
        }
        catch
        {
            // Handle gracefully
        }
    }
    
    public async Task SaveSoundSettingsAsync(bool soundEnabled)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", SoundSettingsKey, soundEnabled.ToString());
        }
        catch
        {
            // Handle gracefully
        }
    }
    
    public async Task<bool> LoadSoundSettingsAsync()
    {
        try
        {
            var value = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", SoundSettingsKey);
            if (bool.TryParse(value, out var soundEnabled))
                return soundEnabled;
        }
        catch
        {
            // Handle gracefully
        }
        return true; // Default to enabled
    }
    
    private List<CellData> SerializeBoard(Cell[,] board)
    {
        var cells = new List<CellData>();
        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                var cell = board[row, col];
                cells.Add(new CellData
                {
                    Row = row,
                    Col = col,
                    IsMine = cell.IsMine,
                    IsRevealed = cell.IsRevealed,
                    IsFlagged = cell.IsFlagged,
                    AdjacentMines = cell.AdjacentMines
                });
            }
        }
        return cells;
    }
}

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