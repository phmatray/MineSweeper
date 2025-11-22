using System.Text.Json;
using Microsoft.JSInterop;
using MineSweeper.Engine.Models;
using MineSweeper.Engine.Services;

namespace MineSweeper.Services;

public class PersistenceService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly GameService _gameService;

    public PersistenceService(IJSRuntime jsRuntime, GameService gameService)
    {
        _jsRuntime = jsRuntime;
        _gameService = gameService;
    }

    public async Task SaveGameStateAsync()
    {
        try
        {
            var savedState = _gameService.SerializeGameState();
            if (savedState == null)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", StorageKeys.CurrentGame);
                return;
            }

            var json = JsonSerializer.Serialize(savedState);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKeys.CurrentGame, json);
        }
        catch
        {
            // Handle gracefully if localStorage is not available
        }
    }

    public async Task<SavedGameState?> LoadGameStateAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKeys.CurrentGame);
            if (string.IsNullOrEmpty(json))
                return null;

            return JsonSerializer.Deserialize<SavedGameState>(json);
        }
        catch
        {
            return null;
        }
    }

    public async Task ClearGameStateAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", StorageKeys.CurrentGame);
        }
        catch
        {
            // Handle gracefully
        }
    }

    public async Task SaveSoundSettingsAsync(bool soundEnabled)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKeys.SoundSettings, soundEnabled.ToString());
        }
        catch
        {
            // Handle gracefully
        }
    }

    public async Task<bool> LoadSoundSettingsAsync()
    {
        try
        {
            var value = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKeys.SoundSettings);
            if (bool.TryParse(value, out var soundEnabled))
                return soundEnabled;
        }
        catch
        {
            // Handle gracefully
        }
        return true; // Default to enabled
    }
}