using Microsoft.AspNetCore.Components;

namespace MineSweeper.Components.Molecules;

public partial class Modal
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public RenderFragment? HeaderContent { get; set; }

    [Parameter] public RenderFragment? FooterContent { get; set; }

    [Parameter] public string? Title { get; set; }

    [Parameter] public string? Icon { get; set; }

    [Parameter] public bool IsVisible { get; set; }

    [Parameter] public bool ShowHeader { get; set; } = true;

    [Parameter] public bool Dismissible { get; set; } = true;

    [Parameter] public bool CloseOnOverlayClick { get; set; } = true;

    [Parameter] public ModalSize Size { get; set; } = ModalSize.Medium;

    [Parameter] public EventCallback OnClose { get; set; }

    [Parameter] public string? Class { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private bool _isClosing = false;

    private async Task Close()
    {
        _isClosing = true;
        StateHasChanged();
        await Task.Delay(200);
        _isClosing = false;
        await OnClose.InvokeAsync();
    }

    private async Task HandleOverlayClick()
    {
        if (CloseOnOverlayClick && Dismissible)
        {
            await Close();
        }
    }

    private string GetSizeClass()
    {
        var sizeClass = Size switch
        {
            ModalSize.Small => "max-w-md",
            ModalSize.Medium => "max-w-2xl",
            ModalSize.Large => "max-w-4xl",
            ModalSize.ExtraLarge => "max-w-6xl",
            ModalSize.Full => "max-w-7xl",
            _ => "max-w-2xl"
        };

        return string.IsNullOrWhiteSpace(Class) ? sizeClass : $"{sizeClass} {Class}";
    }

    private string GetHeaderClasses()
    {
        return "bg-gradient-to-r from-gray-700 to-gray-800 p-6 border-b border-gray-700";
    }

    private string GetBodyClasses()
    {
        return "p-6 overflow-y-auto max-h-[calc(90vh-100px)]";
    }

    private string GetFooterClasses()
    {
        return "bg-gray-800 p-6 border-t border-gray-700 flex justify-end gap-3";
    }

    public enum ModalSize
    {
        Small,
        Medium,
        Large,
        ExtraLarge,
        Full
    }
}