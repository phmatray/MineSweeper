using System.Text.Json;
using Microsoft.JSInterop;
using MineSweeper.Engine.Models;
using MineSweeper.Engine.Services;

namespace MineSweeper.Services;

public class AchievementService : IDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private readonly GameService _gameService;
    private readonly SoundService _soundService;
    private readonly AchievementChecker _achievementChecker;
    
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
        _achievementChecker = new AchievementChecker();

        _gameService.OnGameStateChanged += OnGameStateChanged;
        _ = LoadDataAsync();
    }
    
    private async Task LoadDataAsync()
    {
        try
        {
            var statsJson = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKeys.Statistics);
            if (!string.IsNullOrEmpty(statsJson))
            {
                Statistics = JsonSerializer.Deserialize<GameStatistics>(statsJson) ?? new();
            }

            var achievementsJson = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKeys.Achievements);
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
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKeys.Statistics, statsJson);

            var unlockedIds = UnlockedAchievements.Select(a => a.Id).ToList();
            var achievementsJson = JsonSerializer.Serialize(unlockedIds);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKeys.Achievements, achievementsJson);
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

        var gameEndData = new AchievementChecker.GameEndData(
            game,
            _cellsRevealedOnFirstClick,
            _usedFlagsInCurrentGame,
            playTime
        );

        // Update statistics using the engine
        _achievementChecker.UpdateStatistics(Statistics, gameEndData);

        // Check for achievements using the engine
        var newAchievements = _achievementChecker.CheckAchievements(Statistics, gameEndData, UnlockedAchievements);

        // Process newly unlocked achievements
        foreach (var achievement in newAchievements)
        {
            UnlockedAchievements.Add(achievement);
            OnAchievementUnlocked?.Invoke(achievement);
            if (_soundService.SoundEnabled)
                await _soundService.PlayWinAsync(); // Reuse win sound for achievements
        }

        // Save data
        await SaveDataAsync();
        OnStatisticsUpdated?.Invoke();
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