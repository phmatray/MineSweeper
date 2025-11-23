using Microsoft.AspNetCore.Components;

namespace MineSweeper.UIKit.Components.Molecules;

public partial class StatCard
{
    [Parameter, EditorRequired] public string Label { get; set; } = "";
    [Parameter, EditorRequired] public string Value { get; set; } = "";
    [Parameter] public string? SubValue { get; set; }
    [Parameter] public string ValueColor { get; set; } = "white"; // white, green, red, yellow, blue
    [Parameter] public string Size { get; set; } = "md"; // sm, md, lg
    [Parameter] public bool Monospace { get; set; } = true;

    private string GetLabelClasses()
    {
        return "text-gray-300";
    }

    private string GetValueClasses()
    {
        var classes = "font-bold";

        if (Monospace)
        {
            classes += " font-mono";
        }

        classes += Size switch
        {
            "sm" => " text-lg",
            "md" => " text-2xl",
            "lg" => " text-3xl",
            _ => " text-2xl"
        };

        classes += ValueColor switch
        {
            "white" => " text-white",
            "green" => " text-green-400",
            "red" => " text-red-400",
            "yellow" => " text-yellow-400",
            "blue" => " text-blue-400",
            _ => " text-white"
        };

        return classes;
    }
}