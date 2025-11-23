using Microsoft.AspNetCore.Components;
using MineSweeper.UIKit.Components.Atoms;

namespace MineSweeper.Components.Atoms;

public partial class CardBody
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

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
            CardPadding.Small => "p-4",
            CardPadding.Medium => "p-5",
            CardPadding.Large => "p-6",
            _ => "p-5"
        });

        // Additional custom classes
        if (!string.IsNullOrWhiteSpace(Class))
        {
            classes.Add(Class);
        }

        return string.Join(" ", classes.Where(c => !string.IsNullOrWhiteSpace(c)));
    }
}