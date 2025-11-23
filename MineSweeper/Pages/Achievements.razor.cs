using MineSweeper.Engine.Models;

namespace MineSweeper.Pages;

public partial class Achievements
{
    private Achievement? _recentAchievement;
    private AchievementCategory? _selectedCategory = null;

    protected override void OnInitialized()
    {
        AchievementService.OnAchievementUnlocked += OnAchievementUnlocked;
    }

    private void OnAchievementUnlocked(Achievement achievement)
    {
        _recentAchievement = achievement;
        InvokeAsync(StateHasChanged);
    }

    private void SetFilter(AchievementCategory? category)
    {
        _selectedCategory = category;
    }

    private IEnumerable<Achievement> GetFilteredAchievements()
    {
        var achievements = _selectedCategory.HasValue
            ? MineSweeper.Engine.Models.Achievements.All.Where(a => a.Category == _selectedCategory.Value)
            : MineSweeper.Engine.Models.Achievements.All;

        return achievements.OrderByDescending(a => a.IsUnlocked)
            .ThenBy(a => a.Category)
            .ThenBy(a => a.Name);
    }

    private string GetCategoryIcon(AchievementCategory category)
    {
        return category switch
        {
            AchievementCategory.Speed => "⚡",
            AchievementCategory.Skill => "🎯",
            AchievementCategory.Endurance => "💪",
            AchievementCategory.Special => "⭐",
            _ => "🏆"
        };
    }

    public void Dispose()
    {
        AchievementService.OnAchievementUnlocked -= OnAchievementUnlocked;
    }
}