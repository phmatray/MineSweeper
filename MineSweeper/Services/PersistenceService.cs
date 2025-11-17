using System.Text.Json;
using Microsoft.JSInterop;
using MineSweeper.Engine.Models;
using MineSweeper.Engine.Services;

namespace MineSweeper.Services;

public class PersistenceService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly GameService _gameService;
    private const string GameStateKey = "minesweeper_current_game";
    private const string SoundSettingsKey = "minesweeper_sound_settings";

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
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", GameStateKey);
                return;
            }

            var json = JsonSerializer.Serialize(savedState);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", GameStateKey, json);
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
            var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", GameStateKey);
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
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", GameStateKey);
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
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", SoundSettingsKey, soundEnabled.ToString());
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
            var value = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", SoundSettingsKey);
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