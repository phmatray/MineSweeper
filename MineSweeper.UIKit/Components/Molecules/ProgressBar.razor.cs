using Microsoft.AspNetCore.Components;

namespace MineSweeper.UIKit.Components.Molecules;

public partial class ProgressBar
{
    [Parameter] public double Percentage { get; set; }
    [Parameter] public string? Label { get; set; }
    [Parameter] public bool ShowLabel { get; set; } = true;
    [Parameter] public bool ShowPercentage { get; set; } = true;
    [Parameter] public string Color { get; set; } = "green"; // green, blue, yellow, red
    [Parameter] public string LabelColorClass { get; set; } = "text-white";

    private string GetGradientClass()
    {
        return Color switch
        {
            "green" => "gradient-green",
            "blue" => "gradient-blue",
            "yellow" => "gradient-yellow",
            "red" => "gradient-red",
            _ => "gradient-green"
        };
    }
}