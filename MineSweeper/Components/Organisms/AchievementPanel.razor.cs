using MineSweeper.Engine.Models;

namespace MineSweeper.Components.Organisms;

public partial class AchievementPanel
{
    private bool _showAchievements = true;
    private Achievement? _recentAchievement;
    private Timer? _notificationTimer;

    protected override void OnInitialized()
    {
        AchievementService.OnAchievementUnlocked += OnAchievementUnlocked;
        AchievementService.OnStatisticsUpdated += OnStatisticsUpdated;
    }

    private void OnAchievementUnlocked(Achievement achievement)
    {
        _recentAchievement = achievement;
        InvokeAsync(StateHasChanged);

        _notificationTimer?.Dispose();
        _notificationTimer = new Timer(_ =>
        {
            _recentAchievement = null;
            InvokeAsync(StateHasChanged);
        }, null, TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);
    }

    private void OnStatisticsUpdated()
    {
        InvokeAsync(StateHasChanged);
    }

    private void ToggleView()
    {
        _showAchievements = !_showAchievements;
    }

    private string FormatTime(TimeSpan time)
    {
        return $"{(int)time.TotalMinutes:00}:{time.Seconds:00}";
    }

    private string FormatPlayTime(TimeSpan time)
    {
        return time.TotalHours >= 1
            ? $"{(int)time.TotalHours}h {time.Minutes}m"
            : $"{(int)time.TotalMinutes}m {time.Seconds}s";
    }

    public void Dispose()
    {
        AchievementService.OnAchievementUnlocked -= OnAchievementUnlocked;
        AchievementService.OnStatisticsUpdated -= OnStatisticsUpdated;
        _notificationTimer?.Dispose();
    }
}