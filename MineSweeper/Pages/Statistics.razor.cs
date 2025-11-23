using MineSweeper.Engine.Models;

namespace MineSweeper.Pages;

public partial class Statistics
{
    protected override void OnInitialized()
    {
        AchievementService.OnStatisticsUpdated += OnStatisticsUpdated;
    }

    private void OnStatisticsUpdated()
    {
        InvokeAsync(StateHasChanged);
    }

    private string GetWinRateColor()
    {
        return AchievementService.Statistics.WinRate switch
        {
            >= 75 => "text-green-400",
            >= 50 => "text-yellow-400",
            >= 25 => "text-orange-400",
            _ => "text-red-400"
        };
    }

    private List<(string Name, DifficultyStatistics Stats, string Color, string Icon)> GetDifficultyData()
    {
        return new()
        {
            ("Beginner", AchievementService.Statistics.BeginnerStats, "green", "🌱"),
            ("Intermediate", AchievementService.Statistics.IntermediateStats, "yellow", "🔥"),
            ("Expert", AchievementService.Statistics.ExpertStats, "red", "💀")
        };
    }

    private string FormatTime(TimeSpan time)
    {
        return $"{(int)time.TotalMinutes:00}:{time.Seconds:00}";
    }

    private string FormatPlayTime(TimeSpan time)
    {
        if (time.TotalDays >= 1)
            return $"{(int)time.TotalDays}d {time.Hours}h";
        else if (time.TotalHours >= 1)
            return $"{(int)time.TotalHours}h {time.Minutes}m";
        else
            return $"{(int)time.TotalMinutes}m {time.Seconds}s";
    }

    public void Dispose()
    {
        AchievementService.OnStatisticsUpdated -= OnStatisticsUpdated;
    }
}