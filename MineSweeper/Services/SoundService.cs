using Microsoft.JSInterop;

namespace MineSweeper.Services;

public class SoundService : IDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private readonly PersistenceService _persistenceService;
    private bool _soundEnabled = true;

    public bool SoundEnabled
    {
        get => _soundEnabled;
        set
        {
            _soundEnabled = value;
            OnSoundToggled?.Invoke(value);
            _ = _persistenceService.SaveSoundSettingsAsync(value);
        }
    }

    public event Action<bool>? OnSoundToggled;

    public SoundService(IJSRuntime jsRuntime, PersistenceService persistenceService)
    {
        _jsRuntime = jsRuntime;
        _persistenceService = persistenceService;
        _ = LoadSoundSettingsAsync();
    }
    
    private async Task LoadSoundSettingsAsync()
    {
        _soundEnabled = await _persistenceService.LoadSoundSettingsAsync();
        OnSoundToggled?.Invoke(_soundEnabled);
    }

    public async Task PlayClickAsync()
    {
        if (_soundEnabled)
            await PlaySoundAsync("sounds/click.wav");
    }

    public async Task PlayStartAsync()
    {
        if (_soundEnabled)
            await PlaySoundAsync("sounds/start.wav");
    }

    public async Task PlayWinAsync()
    {
        if (_soundEnabled)
            await PlaySoundAsync("sounds/win.wav");
    }

    public async Task PlayLoseAsync()
    {
        if (_soundEnabled)
            await PlaySoundAsync("sounds/lose_minesweeper.wav");
    }

    private async Task PlaySoundAsync(string soundPath)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("playSound", soundPath);
        }
        catch
        {
            // Silently handle audio playback errors
        }
    }

    public void Dispose()
    {
        OnSoundToggled = null;
    }
}