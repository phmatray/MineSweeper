namespace MineSweeper.Engine.Models;

public class Achievement
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "";
    public bool IsUnlocked { get; set; }
    public DateTime? UnlockedAt { get; set; }
    public AchievementCategory Category { get; set; }
}

public enum AchievementCategory
{
    Speed,
    Skill,
    Endurance,
    Special
}

public static class Achievements
{
    public static readonly List<Achievement> All = new()
    {
        new Achievement
        {
            Id = "first_win",
            Name = "First Victory",
            Description = "Win your first game",
            Icon = "🏆",
            Category = AchievementCategory.Special
        },
        new Achievement
        {
            Id = "speed_demon",
            Name = "Speed Demon",
            Description = "Complete Expert in under 100 seconds",
            Icon = "⚡",
            Category = AchievementCategory.Speed
        },
        new Achievement
        {
            Id = "perfectionist",
            Name = "Perfectionist",
            Description = "Win 10 games without using flags",
            Icon = "✨",
            Category = AchievementCategory.Skill
        },
        new Achievement
        {
            Id = "lucky_start",
            Name = "Lucky Start",
            Description = "Reveal 50+ cells on first click",
            Icon = "🍀",
            Category = AchievementCategory.Special
        },
        new Achievement
        {
            Id = "marathon",
            Name = "Marathon Runner",
            Description = "Play 100 games",
            Icon = "🏃",
            Category = AchievementCategory.Endurance
        },
        new Achievement
        {
            Id = "winning_streak",
            Name = "On Fire",
            Description = "Win 5 games in a row",
            Icon = "🔥",
            Category = AchievementCategory.Skill
        },
        new Achievement
        {
            Id = "beginner_master",
            Name = "Beginner Master",
            Description = "Win Beginner in under 10 seconds",
            Icon = "🌟",
            Category = AchievementCategory.Speed
        },
        new Achievement
        {
            Id = "intermediate_master",
            Name = "Intermediate Master",
            Description = "Win Intermediate in under 40 seconds",
            Icon = "⭐",
            Category = AchievementCategory.Speed
        },
        new Achievement
        {
            Id = "expert_master",
            Name = "Expert Master",
            Description = "Win Expert in under 150 seconds",
            Icon = "💫",
            Category = AchievementCategory.Speed
        },
        new Achievement
        {
            Id = "flag_master",
            Name = "Flag Master",
            Description = "Place exactly the right number of flags and win",
            Icon = "🚩",
            Category = AchievementCategory.Skill
        }
    };
}