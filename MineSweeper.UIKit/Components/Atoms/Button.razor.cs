using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MineSweeper.UIKit.Components.Atoms;

public partial class Button
{
    /// <summary>
    /// The content to display inside the button
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Event callback when the button is clicked
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// The visual variant of the button
    /// </summary>
    [Parameter]
    public ButtonVariant Variant { get; set; } = ButtonVariant.Primary;

    /// <summary>
    /// Whether the button is disabled
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// The HTML button type attribute
    /// </summary>
    [Parameter]
    public string Type { get; set; } = "button";

    /// <summary>
    /// Additional CSS classes to apply to the button
    /// </summary>
    [Parameter]
    public string? AdditionalClasses { get; set; }

    /// <summary>
    /// ARIA label for accessibility
    /// </summary>
    [Parameter]
    public string? AriaLabel { get; set; }

    private string GetButtonClasses()
    {
        var classes = "btn-base";

        classes += Variant switch
        {
            ButtonVariant.Primary => " btn-primary",
            ButtonVariant.Secondary => " btn-secondary",
            ButtonVariant.Success => " btn-success",
            ButtonVariant.Danger => " btn-danger",
            ButtonVariant.Warning => " btn-warning",
            ButtonVariant.Icon => " btn-icon",
            ButtonVariant.Outline => " btn-outline",
            ButtonVariant.Ghost => " btn-ghost",
            _ => " btn-primary"
        };

        if (!string.IsNullOrEmpty(AdditionalClasses))
        {
            classes += " " + AdditionalClasses;
        }

        return classes;
    }
}