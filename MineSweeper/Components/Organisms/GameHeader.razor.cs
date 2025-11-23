using Microsoft.AspNetCore.Components;

namespace MineSweeper.Components.Organisms;

public partial class GameHeader
{
    [Parameter] public EventCallback OnShowHelp { get; set; }

    private void ToggleSound()
    {
        SoundService.SoundEnabled = !SoundService.SoundEnabled;
    }

    private async Task ShowHelp()
    {
        await OnShowHelp.InvokeAsync();
    }
}