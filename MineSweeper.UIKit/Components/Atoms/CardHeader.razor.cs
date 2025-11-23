using Microsoft.AspNetCore.Components;
using MineSweeper.UIKit.Components.Atoms;

namespace MineSweeper.Components.Atoms;

public partial class CardHeader
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public bool Border { get; set; } = true;

    [Parameter] public CardPadding Padding { get; set; } = CardPadding.Medium;

    [Parameter] public string? Class { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string GetClasses()
    {
        var classes = new List<string>();

        // Padding
        classes.Add(Padding switch
        {
            CardPadding.None => "",
            CardPadding.Small => "px-4 py-3",
            CardPadding.Medium => "px-6 py-4",
            CardPadding.Large => "px-8 py-5",
            _ => "px-6 py-4"
        });

        // Border
        if (Border)
        {
            classes.Add("border-b border-gray-700/50");
        }

        // Additional custom classes
        if (!string.IsNullOrWhiteSpace(Class))
        {
            classes.Add(Class);
        }

        return string.Join(" ", classes.Where(c => !string.IsNullOrWhiteSpace(c)));
    }
}