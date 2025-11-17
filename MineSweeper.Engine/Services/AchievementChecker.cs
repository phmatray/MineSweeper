using MineSweeper.Engine.Models;

namespace MineSweeper.Engine.Services;

public class AchievementChecker
{
    public record GameEndData(
        GameState Game,
        int CellsRevealedOnFirstClick,
        bool UsedFlags,
        TimeSpan PlayTime
    );

    public void UpdateStatistics(GameStatistics statistics, GameEndData gameData)
    {
        var game = gameData.Game;

        // Update general statistics
        statistics.TotalGamesPlayed++;
        statistics.TotalCellsRevealed += game.RevealedCells;
        statistics.TotalFlagsPlaced += game.FlaggedCells;
        statistics.TotalPlayTime = statistics.TotalPlayTime.Add(gameData.PlayTime);
        statistics.LastPlayedAt = DateTime.Now;

        if (gameData.CellsRevealedOnFirstClick > statistics.LargestFirstClick)
            statistics.LargestFirstClick = gameData.CellsRevealedOnFirstClick;

        // Update difficulty-specific statistics
        var diffStats = game.Difficulty switch
        {
            GameDifficulty.Beginner => statistics.BeginnerStats,
            GameDifficulty.Intermediate => statistics.IntermediateStats,
            GameDifficulty.Expert => statistics.ExpertStats,
            _ => null
        };

        if (diffStats != null)
        {
            diffStats.GamesPlayed++;

            if (game.Status == GameStatus.Won)
            {
                statistics.TotalWins++;
                diffStats.GamesWon++;
                diffStats.AddGameTime(game.ElapsedTime);

                if (!gameData.UsedFlags)
                    statistics.GamesWonWithoutFlags++;

                statistics.CurrentWinStreak++;
                if (statistics.CurrentWinStreak > statistics.BestWinStreak)
                    statistics.BestWinStreak = statistics.CurrentWinStreak;
            }
            else
            {
                statistics.CurrentWinStreak = 0;
            }
        }
    }

    public List<Achievement> CheckAchievements(GameStatistics statistics, GameEndData gameData, List<Achievement> currentlyUnlocked)
    {
        var game = gameData.Game;
        if (game.Status != GameStatus.Won)
            return new List<Achievement>();

        var newAchievements = new List<Achievement>();
        var unlockedIds = new HashSet<string>(currentlyUnlocked.Select(a => a.Id));

        // First Victory
        CheckAndUnlock("first_win", statistics.TotalWins == 1, unlockedIds, newAchievements);

        // Speed achievements
        if (game.Difficulty == GameDifficulty.Beginner && game.ElapsedTime.TotalSeconds < 10)
            CheckAndUnlock("beginner_master", true, unlockedIds, newAchievements);

        if (game.Difficulty == GameDifficulty.Intermediate && game.ElapsedTime.TotalSeconds < 40)
            CheckAndUnlock("intermediate_master", true, unlockedIds, newAchievements);

        if (game.Difficulty == GameDifficulty.Expert && game.ElapsedTime.TotalSeconds < 150)
            CheckAndUnlock("expert_master", true, unlockedIds, newAchievements);

        if (game.Difficulty == GameDifficulty.Expert && game.ElapsedTime.TotalSeconds < 100)
            CheckAndUnlock("speed_demon", true, unlockedIds, newAchievements);

        // Skill achievements
        CheckAndUnlock("perfectionist", statistics.GamesWonWithoutFlags >= 10, unlockedIds, newAchievements);
        CheckAndUnlock("winning_streak", statistics.CurrentWinStreak >= 5, unlockedIds, newAchievements);

        if (game.FlaggedCells == game.TotalMines)
            CheckAndUnlock("flag_master", true, unlockedIds, newAchievements);

        // Special achievements
        CheckAndUnlock("lucky_start", gameData.CellsRevealedOnFirstClick >= 50, unlockedIds, newAchievements);
        CheckAndUnlock("marathon", statistics.TotalGamesPlayed >= 100, unlockedIds, newAchievements);

        return newAchievements;
    }

    private void CheckAndUnlock(string achievementId, bool condition, HashSet<string> unlockedIds, List<Achievement> newAchievements)
    {
        if (!condition || unlockedIds.Contains(achievementId))
            return;

        var achievement = Achievements.All.FirstOrDefault(a => a.Id == achievementId);
        if (achievement != null)
        {
            var newAchievement = new Achievement
            {
                Id = achievement.Id,
                Name = achievement.Name,
                Description = achievement.Description,
                Icon = achievement.Icon,
                Category = achievement.Category,
                IsUnlocked = true,
                UnlockedAt = DateTime.Now
            };
            newAchievements.Add(newAchievement);
            unlockedIds.Add(achievementId);
        }
    }
}