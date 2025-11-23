using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MineSweeper.Engine.Models;

namespace MineSweeper.Components.Organisms;

public partial class GameBoard
{
    private DateTime _currentGameCreatedAt = DateTime.MinValue;

    protected override void OnInitialized()
    {
        GameService.OnGameStateChanged += OnGameStateChanged;
        if (GameService.CurrentGame != null)
        {
            _currentGameCreatedAt = GameService.CurrentGame.CreatedAt;
        }
    }

    protected override void OnParametersSet()
    {
        // Update the current game reference when parameters change
        if (GameService.CurrentGame != null && GameService.CurrentGame.CreatedAt != _currentGameCreatedAt)
        {
            _currentGameCreatedAt = GameService.CurrentGame.CreatedAt;
            _hasPlayedWinEffect = false;
        }
    }

    private void OnGameStateChanged()
    {
        // Update the current game reference
        if (GameService.CurrentGame != null)
        {
            _currentGameCreatedAt = GameService.CurrentGame.CreatedAt;
        }

        InvokeAsync(StateHasChanged);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JsRuntime.InvokeVoidAsync("ParticleEffects.init");
        }

        // Check for game over states and trigger effects
        if (GameService.CurrentGame != null)
        {
            if (GameService.CurrentGame.Status == GameStatus.Won && !_hasPlayedWinEffect)
            {
                _hasPlayedWinEffect = true;
                await JsRuntime.InvokeVoidAsync("ParticleEffects.createConfetti");
            }
            else if (GameService.CurrentGame.Status == GameStatus.InProgress ||
                     GameService.CurrentGame.Status == GameStatus.NotStarted)
            {
                _hasPlayedWinEffect = false;
            }
        }
    }

    private bool _hasPlayedWinEffect;

    private async void HandleCellClick(int row, int col, MouseEventArgs e)
    {
        var previousStatus = GameService.CurrentGame?.Status;
        var cell = GameService.CurrentGame?.Board[row, col];
        var wasMine = cell?.IsMine ?? false;
        var wasRevealed = cell?.IsRevealed ?? false;

        GameService.RevealCell(row, col);

        if (GameService.CurrentGame?.Status == GameStatus.InProgress ||
            (previousStatus == GameStatus.NotStarted && GameService.CurrentGame?.Status == GameStatus.InProgress))
        {
            await SoundService.PlayClickAsync();

            // Only add sparkle effect for single cell reveals (not batch reveals)
            if (!wasMine && !wasRevealed && GameService.CurrentGame?.Status == GameStatus.InProgress)
            {
                var newCell = GameService.CurrentGame?.Board[row, col];
                if (newCell != null && newCell.AdjacentMines > 0)
                {
                    // Only sparkle for cells with numbers (not empty cells that trigger batch reveal)
                    await JsRuntime.InvokeVoidAsync("ParticleEffects.createSparkle", e.ClientX, e.ClientY);
                }
            }
        }

        // Add explosion effect if mine was hit
        if (wasMine && GameService.CurrentGame?.Status == GameStatus.Lost)
        {
            await JsRuntime.InvokeVoidAsync("ParticleEffects.createExplosion", e.ClientX, e.ClientY);
        }
    }

    private async void HandleRightClick(int row, int col, MouseEventArgs e)
    {
        GameService.ToggleFlag(row, col);
        await SoundService.PlayClickAsync();
    }

    private async void HandleDoubleClick(int row, int col)
    {
        GameService.RevealAdjacentIfSafe(row, col);
        await SoundService.PlayClickAsync();
    }

    private string FormatTime(TimeSpan time)
    {
        return $"{(int)time.TotalMinutes:00}:{time.Seconds:00}";
    }

    public void Dispose()
    {
        GameService.OnGameStateChanged -= OnGameStateChanged;
    }
}