using Microsoft.AspNetCore.Components;

namespace MineSweeper.UIKit.Components.Molecules;

public partial class DifficultyButton
{
    [Parameter, EditorRequired] public string DifficultyName { get; set; } = "";
    [Parameter, EditorRequired] public string Icon { get; set; } = "";
    [Parameter, EditorRequired] public string GridSize { get; set; } = "";
    [Parameter, EditorRequired] public int MineCount { get; set; }
    [Parameter] public bool IsSelected { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public string DifficultyLevel { get; set; } = "beginner"; // beginner, intermediate, expert

    private string GetColorClass()
    {
        return DifficultyLevel switch
        {
            "beginner" => "difficulty-beginner",
            "intermediate" => "difficulty-intermediate",
            "expert" => "difficulty-expert",
            _ => "difficulty-beginner"
        };
    }
}