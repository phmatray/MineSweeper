using Microsoft.AspNetCore.Components;
using MineSweeper.UIKit.Components.Atoms;

namespace MineSweeper.Components.Atoms;

public partial class Card
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public CardVariant Variant { get; set; } = CardVariant.Primary;

    [Parameter] public CardPadding Padding { get; set; } = CardPadding.Medium;

    [Parameter] public bool Hover { get; set; } = false;

    [Parameter] public bool Border { get; set; } = false;

    [Parameter] public string? Class { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string GetCardClasses()
    {
        var classes = new List<string> { "rounded-2xl" };

        // Variant styles
        classes.Add(Variant switch
        {
            CardVariant.Primary => "bg-gray-800 shadow-2xl",
            CardVariant.Secondary => "bg-gray-700",
            CardVariant.Glass => "glass",
            CardVariant.GlassStrong => "glass-strong shadow-2xl",
            CardVariant.Elevated => "card-elevated bg-gray-800",
            CardVariant.Dark => "bg-gray-900 shadow-inner",
            CardVariant.Outline => "bg-transparent border-2 border-gray-700",
            _ => "bg-gray-800"
        });

        // Padding
        classes.Add(Padding switch
        {
            CardPadding.None => "",
            CardPadding.Small => "p-4",
            CardPadding.Medium => "p-5",
            CardPadding.Large => "p-6",
            _ => "p-5"
        });

        // Hover effect
        if (Hover)
        {
            classes.Add("transition-all duration-200 hover:bg-gray-650 hover:shadow-xl");
        }

        // Border
        if (Border)
        {
            classes.Add("border border-blue-500/20");
        }

        // Additional custom classes
        if (!string.IsNullOrWhiteSpace(Class))
        {
            classes.Add(Class);
        }

        return string.Join(" ", classes.Where(c => !string.IsNullOrWhiteSpace(c)));
    }
}