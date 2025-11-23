using Microsoft.AspNetCore.Components;

namespace MineSweeper.Components.Atoms;

public partial class Grid
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public int Columns { get; set; } = 1;

    [Parameter] public int? TabletColumns { get; set; }

    [Parameter] public int? DesktopColumns { get; set; }

    [Parameter] public GridGap Gap { get; set; } = GridGap.Medium;

    [Parameter] public string? Class { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string GetClasses()
    {
        var classes = new List<string> { "grid" };

        // Base columns
        classes.Add($"grid-cols-{Columns}");

        // Tablet columns (md breakpoint)
        if (TabletColumns.HasValue)
        {
            classes.Add($"md:grid-cols-{TabletColumns.Value}");
        }

        // Desktop columns (lg breakpoint)
        if (DesktopColumns.HasValue)
        {
            classes.Add($"lg:grid-cols-{DesktopColumns.Value}");
        }

        // Gap
        classes.Add(Gap switch
        {
            GridGap.None => "gap-0",
            GridGap.ExtraSmall => "gap-1",
            GridGap.Small => "gap-2",
            GridGap.Medium => "gap-4",
            GridGap.Large => "gap-6",
            GridGap.ExtraLarge => "gap-8",
            _ => "gap-4"
        });

        // Additional custom classes
        if (!string.IsNullOrWhiteSpace(Class))
        {
            classes.Add(Class);
        }

        return string.Join(" ", classes.Where(c => !string.IsNullOrWhiteSpace(c)));
    }

    public enum GridGap
    {
        None,
        ExtraSmall,
        Small,
        Medium,
        Large,
        ExtraLarge
    }
}