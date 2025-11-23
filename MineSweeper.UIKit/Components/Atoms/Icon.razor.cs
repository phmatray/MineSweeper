using Microsoft.AspNetCore.Components;

namespace MineSweeper.UIKit.Components.Atoms;

public partial class Icon
{
    [Parameter, EditorRequired] public string Emoji { get; set; } = "";
    [Parameter] public string Size { get; set; } = "md"; // sm, md, lg, xl
    [Parameter] public string? AdditionalClasses { get; set; }

    private string GetIconClasses()
    {
        var classes = Size switch
        {
            "sm" => "text-xl",
            "md" => "text-2xl",
            "lg" => "text-4xl",
            "xl" => "text-5xl",
            _ => "text-2xl"
        };

        if (!string.IsNullOrEmpty(AdditionalClasses))
        {
            classes += " " + AdditionalClasses;
        }

        return classes;
    }
}