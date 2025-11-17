# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

MineSweeper is a Blazor WebAssembly game with a **separated engine architecture**. The codebase is split into two projects:

1. **MineSweeper.Engine** - Pure .NET class library containing all game logic, models, and achievement system
2. **MineSweeper** - Blazor WebAssembly frontend with UI components, platform services (localStorage, audio), and Tailwind CSS styling

This separation allows the game engine to be reused with different frontends (console, WPF, MAUI, Unity, etc.).

## Build and Development Commands

### Standard Build
```bash
# Build entire solution (includes Tailwind CSS compilation)
dotnet build

# Build for release
dotnet publish -c Release
```

### Running Locally
```bash
# Run from solution root
cd MineSweeper
dotnet run

# Or from MineSweeper directory
cd MineSweeper/MineSweeper
dotnet run
```

### Tailwind CSS
The build automatically runs Tailwind CSS via MSBuild targets. Manual commands:
```bash
cd MineSweeper

# Build CSS once
npm run build-css

# Watch for changes during development
npm run watch-css
```

## Architecture

### Engine Layer (MineSweeper.Engine)

**Core Responsibilities:**
- Pure game logic with no UI dependencies
- All models, enums, and game state
- Achievement checking and statistics calculation
- Game state serialization (returns `SavedGameState` for frontends to persist)

**Key Services:**
- `GameService` - Core game loop: mine placement, cell reveal, flag toggle, win/loss detection, flood-fill algorithm
- `AchievementChecker` - Pure functions for checking achievements and updating statistics based on `GameEndData`

**Important Models:**
- `GameState` - Contains `Cell[,]` board, difficulty, timers, status
- `SavedGameState` / `CellData` - Flattened serialization models
- `GameStatistics` / `DifficultyStatistics` - Tracks wins, streaks, best times
- `Achievement` / `Achievements` - Static list of 10 achievements across 4 categories

### Frontend Layer (MineSweeper)

**Core Responsibilities:**
- Blazor UI components and routing
- Platform-specific services (localStorage, sounds, JS interop)
- Event-driven UI updates via `OnGameStateChanged`

**Service Architecture:**
- `GameService` (from Engine) - Singleton, maintains `CurrentGame` state
- `PersistenceService` (Scoped) - Wraps engine's serialization with localStorage
- `AchievementService` (Scoped) - Uses engine's `AchievementChecker`, handles events and persistence
- `SoundService` (Scoped) - Audio playback via JS interop

**Component Structure:**
- `MinesweeperGame.razor` - Main container, orchestrates services, handles game lifecycle
- `GameBoard.razor` - Renders CSS grid, handles clicks (left/right/double), triggers visual effects
- `DifficultySelector.razor`, `GameStats.razor`, `GameHeader.razor` - Smaller focused components
- `AchievementPanel.razor`, `HelpModal.razor` - Feature-specific UI

### Key Patterns

**Event-Driven Updates:**
- Engine's `GameService.OnGameStateChanged` event triggers UI re-renders
- Frontend subscribes in `OnInitializedAsync`, unsubscribes in `Dispose()`
- Achievement service listens to same event to process game ends

**Persistence Flow:**
1. Game state changes → `OnGameStateChanged` fires
2. Frontend calls `PersistenceService.SaveGameStateAsync()`
3. Service calls `GameService.SerializeGameState()` (engine)
4. Result saved to localStorage as JSON

**Batch Reveal Optimization:**
- `GameService.RevealCell()` uses `CollectCellsToReveal()` with HashSet visited tracking
- Prevents infinite recursion, improves performance for large flood-fills
- Single `OnGameStateChanged` event after all cells revealed

## Project Configuration

### Target Framework
- .NET 10.0 (both projects)

### GitHub Pages Deployment
- Configured via `<GHPages>true</GHPages>` and `<GHPagesBase>/MineSweeper/</GHPagesBase>`
- Uses `PublishSPAforGitHubPages.Build` NuGet package
- Automatic deployment via GitHub Actions on push to `main`

### Tailwind CSS Integration
- MSBuild `BeforeTargets="Build"` runs `npm install` and `npm run build-css`
- Input: `Styles/app.css`, Output: `wwwroot/css/app.css`
- Tailwind v4.1.17 via `@tailwindcss/cli`

## Important Implementation Details

### First Click Safety
- Mines are NOT placed until first cell is clicked
- `GameService.StartGame(firstRow, firstCol)` ensures first click is never a mine
- Called automatically by `RevealCell()` if `Status == NotStarted`

### Achievement System Split
- **Engine:** `AchievementChecker.CheckAchievements()` returns list of newly unlocked achievements
- **Frontend:** `AchievementService` passes `GameEndData` to checker, handles notifications and saves to localStorage
- Achievements tracked by ID strings, not object instances (for serialization)

### Game State Lifecycle
- `NotStarted` → `InProgress` (on first click) → `Won` or `Lost`
- `StartTime` set when entering `InProgress`
- `EndTime` set when entering `Won` or `Lost`
- Game state cleared from localStorage on win/loss
- Pending difficulty stored in sessionStorage for page reload after game end

### Double-Click Chord Behavior
- `GameService.RevealAdjacentIfSafe()` reveals neighbors if flagged count matches adjacent mine count
- Prevents misclicks by requiring exact flag placement
- Uses same batch reveal mechanism as regular cell reveal

## Namespaces and Imports

When working across projects:
- Engine types: `using MineSweeper.Engine.Models;` and `using MineSweeper.Engine.Services;`
- Frontend services: `using MineSweeper.Services;` (PersistenceService, SoundService, AchievementService)
- Global imports in `_Imports.razor` include both engine namespaces

## Testing and Debugging

### Running in Browser
- Default URL: `https://localhost:5001`
- Game state persists in browser's localStorage (F12 → Application → Local Storage)
- Statistics and achievements also in localStorage

### Common Data Keys
- `minesweeper_current_game` - Saved game state
- `minesweeper_statistics` - Game statistics JSON
- `minesweeper_achievements` - Array of unlocked achievement IDs
- `minesweeper_sound_settings` - Sound enabled boolean

## Adding New Features

### Adding New Difficulty Level
1. Add enum value to `GameDifficulty` (Engine)
2. Update `DifficultySettings.GetSettings()` switch
3. Add to `DifficultySelector.razor` UI
4. Add corresponding `DifficultyStatistics` property to `GameStatistics`

### Adding New Achievement
1. Add achievement definition to `Achievements.All` list (Engine)
2. Add checking logic in `AchievementChecker.CheckAchievements()`
3. Track necessary metrics in `GameStatistics` or `GameEndData`
4. Achievement automatically appears in UI (dynamic rendering)

### Adding New Frontend
1. Reference `MineSweeper.Engine` project
2. Implement your own UI layer
3. Subscribe to `GameService.OnGameStateChanged` for updates
4. Implement your own persistence (file, database, etc.) using `SerializeGameState()`
5. Optionally use `AchievementChecker` for statistics tracking