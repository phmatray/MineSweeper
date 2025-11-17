namespace MineSweeper.Engine.Models;

public class GameStatistics
{
    public int TotalGamesPlayed { get; set; }
    public int TotalWins { get; set; }
    public int TotalLosses => TotalGamesPlayed - TotalWins;
    public double WinRate => TotalGamesPlayed > 0 ? (double)TotalWins / TotalGamesPlayed * 100 : 0;

    public int CurrentWinStreak { get; set; }
    public int BestWinStreak { get; set; }

    public DifficultyStatistics BeginnerStats { get; set; } = new();
    public DifficultyStatistics IntermediateStats { get; set; } = new();
    public DifficultyStatistics ExpertStats { get; set; } = new();

    public int TotalFlagsPlaced { get; set; }
    public int TotalCellsRevealed { get; set; }
    public int GamesWonWithoutFlags { get; set; }
    public int LargestFirstClick { get; set; }

    public DateTime? LastPlayedAt { get; set; }
    public TimeSpan TotalPlayTime { get; set; }
}

public class DifficultyStatistics
{
    public int GamesPlayed { get; set; }
    public int GamesWon { get; set; }
    public TimeSpan? BestTime { get; set; }
    public TimeSpan? AverageTime { get; set; }
    public List<TimeSpan> RecentTimes { get; set; } = new();

    public void AddGameTime(TimeSpan time)
    {
        RecentTimes.Add(time);
        if (RecentTimes.Count > 10)
            RecentTimes.RemoveAt(0);

        if (!BestTime.HasValue || time < BestTime.Value)
            BestTime = time;

        AverageTime = TimeSpan.FromSeconds(RecentTimes.Average(t => t.TotalSeconds));
    }
}