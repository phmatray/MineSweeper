using Microsoft.AspNetCore.Components;
using MineSweeper.Engine.Models;

namespace MineSweeper.Components.Organisms;

public partial class GameStats
{
    [Parameter] public EventCallback OnReset { get; set; }

    private async Task Reset()
    {
        await OnReset.InvokeAsync();
    }

    private string GetFaceEmoji()
    {
        if (GameService.CurrentGame == null) return "😊";

        return GameService.CurrentGame.Status switch
        {
            GameStatus.Won => "😎",
            GameStatus.Lost => "😵",
            _ => "😊"
        };
    }

    private string FormatTime(TimeSpan time)
    {
        return $"{(int)time.TotalMinutes:00}:{time.Seconds:00}";
    }
}