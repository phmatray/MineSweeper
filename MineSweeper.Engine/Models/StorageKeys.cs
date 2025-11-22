namespace MineSweeper.Engine.Models;

/// <summary>
/// Standard storage keys for persisting MineSweeper game data across different frontend implementations.
/// These keys ensure consistency when using localStorage, files, or other persistence mechanisms.
/// </summary>
public static class StorageKeys
{
    /// <summary>
    /// Key for storing the current active game state
    /// </summary>
    public const string CurrentGame = "minesweeper_current_game";

    /// <summary>
    /// Key for storing game statistics
    /// </summary>
    public const string Statistics = "minesweeper_statistics";

    /// <summary>
    /// Key for storing unlocked achievements
    /// </summary>
    public const string Achievements = "minesweeper_achievements";

    /// <summary>
    /// Key for storing sound settings
    /// </summary>
    public const string SoundSettings = "minesweeper_sound_settings";

    /// <summary>
    /// Key for storing pending game difficulty (used for page reloads)
    /// </summary>
    public const string PendingGameDifficulty = "pendingGameDifficulty";
}
