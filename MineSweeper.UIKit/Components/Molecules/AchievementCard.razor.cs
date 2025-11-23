using Microsoft.AspNetCore.Components;

namespace MineSweeper.UIKit.Components.Molecules;

public partial class AchievementCard
{
    [Parameter, EditorRequired] public string Name { get; set; } = "";
    [Parameter, EditorRequired] public string Description { get; set; } = "";
    [Parameter, EditorRequired] public string Icon { get; set; } = "";
    [Parameter] public bool IsUnlocked { get; set; }
    [Parameter] public DateTime? UnlockedAt { get; set; }
    [Parameter] public string? Category { get; set; }
    [Parameter] public string? CategoryIcon { get; set; }
}