using Microsoft.JSInterop;
using MineSweeper.Engine.Models;

namespace MineSweeper.Components.Organisms;

public partial class MinesweeperGame
{
    private Timer? _timer;
    private bool _gameRestored = false;
    private bool _showHelp = false;

    protected override async Task OnInitializedAsync()
    {
        GameService.OnGameStateChanged += OnGameStateChanged;
        SoundService.OnSoundToggled += OnSoundToggled;
        _timer = new Timer(_ => InvokeAsync(StateHasChanged), null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

        // Check if we have a pending game to start after reload
        try
        {
            var pendingDifficulty =
                await JsRuntime.InvokeAsync<string?>("sessionStorage.getItem", StorageKeys.PendingGameDifficulty);
            if (!string.IsNullOrEmpty(pendingDifficulty))
            {
                await JsRuntime.InvokeVoidAsync("sessionStorage.removeItem", StorageKeys.PendingGameDifficulty);
                if (Enum.TryParse<GameDifficulty>(pendingDifficulty, out var difficulty))
                {
                    GameService.NewGame(difficulty);
                    await SoundService.PlayStartAsync();
                    return;
                }
            }
        }
        catch
        {
        }

        // Load saved game if exists
        var savedGame = await PersistenceService.LoadGameStateAsync();
        if (savedGame != null && savedGame.Status == GameStatus.InProgress)
        {
            GameService.LoadGame(savedGame);
            _gameRestored = true;
            // Hide message after 3 seconds
            _ = Task.Delay(3000).ContinueWith(_ =>
            {
                _gameRestored = false;
                InvokeAsync(StateHasChanged);
            });
        }
    }

    private async void OnGameStateChanged()
    {
        await InvokeAsync(StateHasChanged);

        // Save game state
        await PersistenceService.SaveGameStateAsync();

        if (GameService.CurrentGame != null)
        {
            if (GameService.CurrentGame.Status == GameStatus.Won)
            {
                await SoundService.PlayWinAsync();
                await PersistenceService.ClearGameStateAsync();
            }
            else if (GameService.CurrentGame.Status == GameStatus.Lost)
            {
                await SoundService.PlayLoseAsync();
                await PersistenceService.ClearGameStateAsync();
            }
        }
    }

    private async void StartNewGame(GameDifficulty difficulty)
    {
        // Check if we need to force reload (when game is over)
        if (GameService.CurrentGame != null &&
            (GameService.CurrentGame.Status == GameStatus.Won || GameService.CurrentGame.Status == GameStatus.Lost))
        {
            // Save the difficulty to start after reload
            await JsRuntime.InvokeVoidAsync("sessionStorage.setItem", StorageKeys.PendingGameDifficulty,
                difficulty.ToString());

            // Force page reload
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
            return;
        }

        // Normal new game start (first game or resetting during play)
        GameService.NewGame(difficulty);
        await SoundService.PlayStartAsync();
    }

    private void ResetCurrentGame()
    {
        if (GameService.CurrentGame != null)
        {
            StartNewGame(GameService.CurrentGame.Difficulty);
        }
    }

    private void ShowHelp()
    {
        _showHelp = true;
    }

    private void ToggleSound()
    {
        SoundService.SoundEnabled = !SoundService.SoundEnabled;
    }

    private void OnSoundToggled(bool isEnabled)
    {
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        GameService.OnGameStateChanged -= OnGameStateChanged;
        SoundService.OnSoundToggled -= OnSoundToggled;
        _timer?.Dispose();
    }
}