using System.Text.Json;
using Microsoft.JSInterop;
using MineSweeper.Models;

namespace MineSweeper.Services;

public class AchievementService : IDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private readonly GameService _gameService;
    private readonly SoundService _soundService;
    
    private const string StatisticsKey = "minesweeper_statistics";
    private const string AchievementsKey = "minesweeper_achievements";
    
    public GameStatistics Statistics { get; private set; } = new();
    public List<Achievement> UnlockedAchievements { get; private set; } = new();
    public event Action<Achievement>? OnAchievementUnlocked;
    public event Action? OnStatisticsUpdated;
    
    private int _cellsRevealedInCurrentGame;
    private bool _usedFlagsInCurrentGame;
    private int _cellsRevealedOnFirstClick;
    private DateTime? _gameStartTime;

    public AchievementService(IJSRuntime jsRuntime, GameService gameService, SoundService soundService)
    {
        _jsRuntime = jsRuntime;
        _gameService = gameService;
        _soundService = soundService;
        
        _gameService.OnGameStateChanged += OnGameStateChanged;
        _ = LoadDataAsync();
    }
    
    private async Task LoadDataAsync()
    {
        try
        {
            var statsJson = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StatisticsKey);
            if (!string.IsNullOrEmpty(statsJson))
            {
                Statistics = JsonSerializer.Deserialize<GameStatistics>(statsJson) ?? new();
            }
            
            var achievementsJson = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", AchievementsKey);
            if (!string.IsNullOrEmpty(achievementsJson))
            {
                var unlockedIds = JsonSerializer.Deserialize<List<string>>(achievementsJson) ?? new();
                foreach (var id in unlockedIds)
                {
                    var achievement = Achievements.All.FirstOrDefault(a => a.Id == id);
                    if (achievement != null)
                    {
                        achievement.IsUnlocked = true;
                        UnlockedAchievements.Add(achievement);
                    }
                }
            }
        }
        catch
        {
            // Handle gracefully if localStorage is not available
        }
    }
    
    private async Task SaveDataAsync()
    {
        try
        {
            var statsJson = JsonSerializer.Serialize(Statistics);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StatisticsKey, statsJson);
            
            var unlockedIds = UnlockedAchievements.Select(a => a.Id).ToList();
            var achievementsJson = JsonSerializer.Serialize(unlockedIds);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", AchievementsKey, achievementsJson);
        }
        catch
        {
            // Handle gracefully if localStorage is not available
        }
    }
    
    private void OnGameStateChanged()
    {
        var game = _gameService.CurrentGame;
        if (game == null) return;
        
        switch (game.Status)
        {
            case GameStatus.NotStarted:
                _cellsRevealedInCurrentGame = 0;
                _usedFlagsInCurrentGame = false;
                _cellsRevealedOnFirstClick = 0;
                break;
                
            case GameStatus.InProgress:
                if (_gameStartTime == null)
                {
                    _gameStartTime = DateTime.Now;
                    _cellsRevealedOnFirstClick = game.RevealedCells;
                }
                
                if (game.FlaggedCells > 0)
                    _usedFlagsInCurrentGame = true;
                    
                _cellsRevealedInCurrentGame = game.RevealedCells;
                break;
                
            case GameStatus.Won:
            case GameStatus.Lost:
                _ = ProcessGameEndAsync(game);
                break;
        }
    }
    
    private async Task ProcessGameEndAsync(GameState game)
    {
        if (_gameStartTime == null) return;
        
        var playTime = DateTime.Now - _gameStartTime.Value;
        _gameStartTime = null;
        
        // Update general statistics
        Statistics.TotalGamesPlayed++;
        Statistics.TotalCellsRevealed += _cellsRevealedInCurrentGame;
        Statistics.TotalFlagsPlaced += game.FlaggedCells;
        Statistics.TotalPlayTime = Statistics.TotalPlayTime.Add(playTime);
        Statistics.LastPlayedAt = DateTime.Now;
        
        if (_cellsRevealedOnFirstClick > Statistics.LargestFirstClick)
            Statistics.LargestFirstClick = _cellsRevealedOnFirstClick;
        
        // Update difficulty-specific statistics
        var diffStats = game.Difficulty switch
        {
            GameDifficulty.Beginner => Statistics.BeginnerStats,
            GameDifficulty.Intermediate => Statistics.IntermediateStats,
            GameDifficulty.Expert => Statistics.ExpertStats,
            _ => null
        };
        
        if (diffStats != null)
        {
            diffStats.GamesPlayed++;
            
            if (game.Status == GameStatus.Won)
            {
                Statistics.TotalWins++;
                diffStats.GamesWon++;
                diffStats.AddGameTime(game.ElapsedTime);
                
                if (!_usedFlagsInCurrentGame)
                    Statistics.GamesWonWithoutFlags++;
                
                Statistics.CurrentWinStreak++;
                if (Statistics.CurrentWinStreak > Statistics.BestWinStreak)
                    Statistics.BestWinStreak = Statistics.CurrentWinStreak;
            }
            else
            {
                Statistics.CurrentWinStreak = 0;
            }
        }
        
        // Check for achievements
        await CheckAchievementsAsync(game);
        
        // Save data
        await SaveDataAsync();
        OnStatisticsUpdated?.Invoke();
    }
    
    private async Task CheckAchievementsAsync(GameState game)
    {
        if (game.Status != GameStatus.Won) return;
        
        var newAchievements = new List<Achievement>();
        
        // First Victory
        await CheckAndUnlockAsync("first_win", Statistics.TotalWins == 1, newAchievements);
        
        // Speed achievements
        if (game.Difficulty == GameDifficulty.Beginner && game.ElapsedTime.TotalSeconds < 10)
            await CheckAndUnlockAsync("beginner_master", true, newAchievements);
            
        if (game.Difficulty == GameDifficulty.Intermediate && game.ElapsedTime.TotalSeconds < 40)
            await CheckAndUnlockAsync("intermediate_master", true, newAchievements);
            
        if (game.Difficulty == GameDifficulty.Expert && game.ElapsedTime.TotalSeconds < 150)
            await CheckAndUnlockAsync("expert_master", true, newAchievements);
            
        if (game.Difficulty == GameDifficulty.Expert && game.ElapsedTime.TotalSeconds < 100)
            await CheckAndUnlockAsync("speed_demon", true, newAchievements);
        
        // Skill achievements
        await CheckAndUnlockAsync("perfectionist", Statistics.GamesWonWithoutFlags >= 10, newAchievements);
        await CheckAndUnlockAsync("winning_streak", Statistics.CurrentWinStreak >= 5, newAchievements);
        
        if (game.FlaggedCells == game.TotalMines)
            await CheckAndUnlockAsync("flag_master", true, newAchievements);
        
        // Special achievements
        await CheckAndUnlockAsync("lucky_start", _cellsRevealedOnFirstClick >= 50, newAchievements);
        await CheckAndUnlockAsync("marathon", Statistics.TotalGamesPlayed >= 100, newAchievements);
        
        // Notify about new achievements
        foreach (var achievement in newAchievements)
        {
            OnAchievementUnlocked?.Invoke(achievement);
            if (_soundService.SoundEnabled)
                await _soundService.PlayWinAsync(); // Reuse win sound for achievements
        }
    }
    
    private async Task CheckAndUnlockAsync(string achievementId, bool condition, List<Achievement> newAchievements)
    {
        if (!condition) return;
        
        var achievement = Achievements.All.FirstOrDefault(a => a.Id == achievementId);
        if (achievement != null && !achievement.IsUnlocked)
        {
            achievement.IsUnlocked = true;
            achievement.UnlockedAt = DateTime.Now;
            UnlockedAchievements.Add(achievement);
            newAchievements.Add(achievement);
            await SaveDataAsync();
        }
    }
    
    public int GetAchievementProgress()
    {
        return UnlockedAchievements.Count;
    }
    
    public int GetTotalAchievements()
    {
        return Achievements.All.Count;
    }
    
    public void Dispose()
    {
        _gameService.OnGameStateChanged -= OnGameStateChanged;
        OnAchievementUnlocked = null;
        OnStatisticsUpdated = null;
    }
}