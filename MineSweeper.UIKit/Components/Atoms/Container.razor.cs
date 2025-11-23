using Microsoft.AspNetCore.Components;

namespace MineSweeper.Components.Atoms;

public partial class Container
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public ContainerSize MaxWidth { get; set; } = ContainerSize.SevenXL;

    [Parameter] public ContainerPadding Padding { get; set; } = ContainerPadding.Medium;

    [Parameter] public bool CenterContent { get; set; } = true;

    [Parameter] public string? Class { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string GetClasses()
    {
        var classes = new List<string>();

        // Max width
        classes.Add(MaxWidth switch
        {
            ContainerSize.Small => "max-w-sm",
            ContainerSize.Medium => "max-w-md",
            ContainerSize.Large => "max-w-lg",
            ContainerSize.ExtraLarge => "max-w-xl",
            ContainerSize.TwoXL => "max-w-2xl",
            ContainerSize.ThreeXL => "max-w-3xl",
            ContainerSize.FourXL => "max-w-4xl",
            ContainerSize.FiveXL => "max-w-5xl",
            ContainerSize.SixXL => "max-w-6xl",
            ContainerSize.SevenXL => "max-w-7xl",
            ContainerSize.Full => "max-w-full",
            _ => "max-w-7xl"
        });

        // Center content
        if (CenterContent)
        {
            classes.Add("mx-auto");
        }

        // Padding
        classes.Add(Padding switch
        {
            ContainerPadding.None => "",
            ContainerPadding.Small => "px-3 py-3",
            ContainerPadding.Medium => "px-4 py-6",
            ContainerPadding.Large => "px-6 py-8",
            _ => "px-4 py-6"
        });

        // Additional custom classes
        if (!string.IsNullOrWhiteSpace(Class))
        {
            classes.Add(Class);
        }

        return string.Join(" ", classes.Where(c => !string.IsNullOrWhiteSpace(c)));
    }

    public enum ContainerSize
    {
        Small,
        Medium,
        Large,
        ExtraLarge,
        TwoXL,
        ThreeXL,
        FourXL,
        FiveXL,
        SixXL,
        SevenXL,
        Full
    }

    public enum ContainerPadding
    {
        None,
        Small,
        Medium,
        Large
    }
}