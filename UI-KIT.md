# MineSweeper UI Kit Documentation

This document provides comprehensive guidance on using the UI Kit components created for the MineSweeper Blazor application.

## Table of Contents

1. [Layout Components](#layout-components)
2. [Card Components](#card-components)
3. [Typography Components](#typography-components)
4. [Navigation Components](#navigation-components)
5. [Feedback Components](#feedback-components)
6. [Examples](#examples)

---

## Layout Components

### Container

A responsive container component that centers and constrains content width.

**Location:** `MineSweeper/Components/Atoms/Container.razor`

**Props:**
- `MaxWidth` - ContainerSize (Small | Medium | Large | ExtraLarge | TwoXL | ThreeXL | FourXL | FiveXL | SixXL | SevenXL | Full) - Default: `SevenXL`
- `Padding` - ContainerPadding (None | Small | Medium | Large) - Default: `Medium`
- `CenterContent` - bool - Default: `true`
- `Class` - string - Additional CSS classes

**Example:**
```razor
<Container MaxWidth="Container.ContainerSize.FiveXL" Padding="Container.ContainerPadding.Large">
    <!-- Your content here -->
</Container>
```

---

### Stack

A flexbox layout component for arranging children vertically or horizontally.

**Location:** `MineSweeper/Components/Atoms/Stack.razor`

**Props:**
- `Direction` - StackDirection (Row | RowReverse | Column | ColumnReverse) - Default: `Column`
- `Align` - StackAlign (Start | Center | End | Stretch | Baseline) - Default: `Start`
- `Justify` - StackJustify (Start | Center | End | Between | Around | Evenly) - Default: `Start`
- `Gap` - StackGap (None | ExtraSmall | Small | Medium | Large | ExtraLarge | TwoXL) - Default: `Medium`
- `Wrap` - bool - Default: `false`
- `Class` - string - Additional CSS classes

**Example:**
```razor
<Stack Direction="Stack.StackDirection.Row"
       Align="Stack.StackAlign.Center"
       Gap="Stack.StackGap.Medium">
    <span>Item 1</span>
    <span>Item 2</span>
</Stack>
```

---

### Grid

A CSS Grid layout component for creating responsive grid layouts.

**Location:** `MineSweeper/Components/Atoms/Grid.razor`

**Props:**
- `Columns` - int - Number of columns (mobile) - Default: `1`
- `TabletColumns` - int? - Number of columns on tablets (optional)
- `DesktopColumns` - int? - Number of columns on desktop (optional)
- `Gap` - GridGap (None | ExtraSmall | Small | Medium | Large | ExtraLarge) - Default: `Medium`
- `Class` - string - Additional CSS classes

**Example:**
```razor
<Grid Columns="1" TabletColumns="2" DesktopColumns="3" Gap="Grid.GridGap.Medium">
    <div>Column 1</div>
    <div>Column 2</div>
    <div>Column 3</div>
</Grid>
```

---

## Card Components

### Card

A versatile card component with multiple variants and customization options.

**Location:** `MineSweeper/Components/Atoms/Card.razor`

**Props:**
- `Variant` - CardVariant (Primary | Secondary | Glass | GlassStrong | Elevated | Dark | Outline) - Default: `Primary`
- `Padding` - CardPadding (None | Small | Medium | Large) - Default: `Medium`
- `Hover` - bool - Enable hover effect - Default: `false`
- `Border` - bool - Add border - Default: `false`
- `Class` - string - Additional CSS classes

**Example:**
```razor
<Card Variant="CardVariant.Primary" Padding="CardPadding.Medium" Hover="true">
    <p>Card content goes here</p>
</Card>
```

---

### CardHeader

A card header component with consistent styling.

**Location:** `MineSweeper/Components/Atoms/CardHeader.razor`

**Props:**
- `Border` - bool - Show bottom border - Default: `true`
- `Padding` - CardPadding (None | Small | Medium | Large) - Default: `Medium`
- `Class` - string - Additional CSS classes

**Example:**
```razor
<Card>
    <CardHeader>
        <h3>Card Title</h3>
    </CardHeader>
    <CardBody>
        <p>Content</p>
    </CardBody>
</Card>
```

---

### CardBody

A card body component for main content.

**Location:** `MineSweeper/Components/Atoms/CardBody.razor`

**Props:**
- `Padding` - CardPadding (None | Small | Medium | Large) - Default: `Medium`
- `Class` - string - Additional CSS classes

---

### CardFooter

A card footer component with consistent styling.

**Location:** `MineSweeper/Components/Atoms/CardFooter.razor`

**Props:**
- `Border` - bool - Show top border - Default: `true`
- `Padding` - CardPadding (None | Small | Medium | Large) - Default: `Medium`
- `Class` - string - Additional CSS classes

**Example:**
```razor
<Card>
    <CardBody>
        <p>Content</p>
    </CardBody>
    <CardFooter>
        <Button>Action</Button>
    </CardFooter>
</Card>
```

---

## Typography Components

### SectionHeader

A consistent heading component with optional icon.

**Location:** `MineSweeper/Components/Atoms/SectionHeader.razor`

**Props:**
- `Title` - string - The header text
- `Icon` - string - Emoji or icon (optional)
- `Size` - HeaderSize (ExtraSmall | Small | Medium | Large | ExtraLarge) - Default: `Large`
- `Class` - string - Additional CSS classes

**Example:**
```razor
<SectionHeader Icon="📊" Title="Statistics" Size="SectionHeader.HeaderSize.Large" />
```

---

## Navigation Components

### NavItem

A navigation link component for consistent nav styling.

**Location:** `MineSweeper/Components/Atoms/NavItem.razor`

**Props:**
- `Href` - string - Navigation URL (required)
- `Label` - string - Link text
- `Icon` - string - Emoji or icon (optional)
- `Match` - NavLinkMatch - Matching behavior - Default: `Prefix`
- `Size` - NavItemSize (Small | Medium | Large) - Default: `Medium`
- `IsMobile` - bool - Use mobile styling - Default: `false`
- `OnClick` - EventCallback - Click handler (optional)
- `Class` - string - Additional CSS classes

**Example:**
```razor
<!-- Desktop Navigation -->
<NavItem Href="stats" Icon="📊" Label="Statistics" />

<!-- Mobile Navigation -->
<NavItem Href="stats" Icon="📊" Label="Statistics" IsMobile="true" OnClick="CloseMenu" />
```

---

## Feedback Components

### Modal

A reusable modal/dialog component with customizable content.

**Location:** `MineSweeper/Components/Molecules/Modal.razor`

**Props:**
- `IsVisible` - bool - Control modal visibility (required)
- `OnClose` - EventCallback - Close event handler (required)
- `Title` - string - Modal title (optional)
- `Icon` - string - Title icon (optional)
- `ShowHeader` - bool - Show header section - Default: `true`
- `Dismissible` - bool - Allow closing - Default: `true`
- `CloseOnOverlayClick` - bool - Close when clicking overlay - Default: `true`
- `Size` - ModalSize (Small | Medium | Large | ExtraLarge | Full) - Default: `Medium`
- `HeaderContent` - RenderFragment - Custom header (optional)
- `FooterContent` - RenderFragment - Custom footer (optional)
- `Class` - string - Additional CSS classes

**Example:**
```razor
<Modal IsVisible="@_showModal"
       OnClose="@CloseModal"
       Title="Confirmation"
       Icon="⚠️"
       Size="Modal.ModalSize.Medium">
    <p>Are you sure you want to proceed?</p>
</Modal>

@code {
    private bool _showModal = false;

    private void CloseModal()
    {
        _showModal = false;
    }
}
```

---

### Toast

A notification component for displaying temporary messages.

**Location:** `MineSweeper/Components/Molecules/Toast.razor`

**Props:**
- `IsVisible` - bool - Control toast visibility - Default: `true`
- `Title` - string - Toast title (optional)
- `Message` - string - Toast message (optional)
- `Icon` - string - Emoji or icon (optional)
- `Type` - ToastType (Success | Error | Warning | Info) - Default: `Info`
- `Dismissible` - bool - Show close button - Default: `true`
- `AutoDismissMs` - int? - Auto-dismiss after milliseconds (optional)
- `OnDismiss` - EventCallback - Dismiss event handler
- `Class` - string - Additional CSS classes

**Example:**
```razor
<div class="fixed bottom-4 right-4 z-50">
    <Toast IsVisible="@_showToast"
           Icon="🏆"
           Title="Achievement Unlocked!"
           Message="You completed your first game!"
           Type="Toast.ToastType.Success"
           AutoDismissMs="5000"
           OnDismiss="@(() => _showToast = false)" />
</div>

@code {
    private bool _showToast = false;
}
```

---

## Examples

### Example 1: Statistics Page Layout

```razor
<Container>
    <Card Variant="CardVariant.Primary">
        <SectionHeader Icon="📊" Title="Game Statistics" Size="SectionHeader.HeaderSize.Large" />

        <Grid Columns="1" TabletColumns="2" DesktopColumns="3" Gap="Grid.GridGap.Medium">
            <Card Variant="CardVariant.Secondary" Padding="CardPadding.Small" Hover="true">
                <SectionHeader Icon="🎯" Title="Overall Performance" Size="SectionHeader.HeaderSize.Small" />
                <Stack Direction="Stack.StackDirection.Column" Gap="Stack.StackGap.Medium">
                    <Stack Direction="Stack.StackDirection.Row" Justify="Stack.StackJustify.Between">
                        <span>Total Games</span>
                        <span class="font-bold">42</span>
                    </Stack>
                </Stack>
            </Card>
        </Grid>
    </Card>
</Container>
```

### Example 2: Modal Dialog

```razor
<Button OnClick="@(() => _showHelp = true)">Show Help</Button>

<Modal IsVisible="@_showHelp"
       OnClose="@(() => _showHelp = false)"
       Title="How to Play"
       Icon="📖"
       Size="Modal.ModalSize.Large">
    <Stack Direction="Stack.StackDirection.Column" Gap="Stack.StackGap.Large">
        <section>
            <SectionHeader Icon="🎯" Title="Objective" Size="SectionHeader.HeaderSize.Medium" />
            <p>Clear all safe cells without hitting mines.</p>
        </section>

        <section>
            <SectionHeader Icon="🎮" Title="Controls" Size="SectionHeader.HeaderSize.Medium" />
            <Stack Direction="Stack.StackDirection.Column" Gap="Stack.StackGap.Medium">
                <Card Variant="CardVariant.Secondary" Padding="CardPadding.Small">
                    <Stack Direction="Stack.StackDirection.Row" Gap="Stack.StackGap.Medium">
                        <span class="text-blue-400 font-mono">Left Click</span>
                        <span>Reveal a cell</span>
                    </Stack>
                </Card>
            </Stack>
        </section>
    </Stack>
</Modal>
```

### Example 3: Navigation Layout

```razor
<nav class="bg-slate-900 border-b border-blue-500/20">
    <Container>
        <Stack Direction="Stack.StackDirection.Row" Justify="Stack.StackJustify.Between" Align="Stack.StackAlign.Center">
            <a href="">Logo</a>

            <!-- Desktop Navigation -->
            <Stack Direction="Stack.StackDirection.Row" Gap="Stack.StackGap.Small" Class="hidden md:flex">
                <NavItem Href="" Match="NavLinkMatch.All" Icon="🎮" Label="Play" />
                <NavItem Href="stats" Icon="📊" Label="Statistics" />
                <NavItem Href="achievements" Icon="🏆" Label="Achievements" />
            </Stack>

            <!-- Mobile Menu Toggle -->
            <Button Variant="ButtonVariant.Icon" Class="md:hidden">☰</Button>
        </Stack>

        <!-- Mobile Navigation -->
        @if (_showMobileMenu)
        {
            <Stack Direction="Stack.StackDirection.Column" Gap="Stack.StackGap.Small" Class="md:hidden pb-4">
                <NavItem Href="" Match="NavLinkMatch.All" Icon="🎮" Label="Play" IsMobile="true" OnClick="CloseMenu" />
                <NavItem Href="stats" Icon="📊" Label="Statistics" IsMobile="true" OnClick="CloseMenu" />
                <NavItem Href="achievements" Icon="🏆" Label="Achievements" IsMobile="true" OnClick="CloseMenu" />
            </Stack>
        }
    </Container>
</nav>
```

### Example 4: Achievement Notification

```razor
<div class="fixed bottom-4 right-4 z-50 max-w-md">
    <Toast IsVisible="@(_recentAchievement != null)"
           Icon="@(_recentAchievement?.Icon)"
           Title="Achievement Unlocked!"
           Message="@(_recentAchievement?.Name)"
           Type="Toast.ToastType.Success"
           AutoDismissMs="5000"
           OnDismiss="@(() => _recentAchievement = null)" />
</div>
```

---

## Component Organization

All UI Kit components are organized following atomic design principles:

### Atoms (Basic Building Blocks)
- **MineSweeper/Components/Atoms/**
  - `Button.razor` - Button component with variants
  - `Badge.razor` - Badge/label component
  - `Icon.razor` - Icon wrapper
  - `Card.razor` - Card container
  - `CardHeader.razor` - Card header section
  - `CardBody.razor` - Card body section
  - `CardFooter.razor` - Card footer section
  - `CardVariant.cs` - Card variant enums
  - `SectionHeader.razor` - Section heading component
  - `Container.razor` - Page container
  - `Stack.razor` - Flexbox layout
  - `Grid.razor` - Grid layout
  - `NavItem.razor` - Navigation link

### Molecules (Component Combinations)
- **MineSweeper/Components/Molecules/**
  - `Modal.razor` - Modal/dialog component
  - `Toast.razor` - Notification component
  - `StatCard.razor` - Statistics card
  - `AchievementCard.razor` - Achievement display card
  - `DifficultyButton.razor` - Difficulty selector button
  - `ProgressBar.razor` - Progress indicator
  - `GameCell.razor` - Minesweeper cell

### Organisms (Complex Components)
- **MineSweeper/Components/Organisms/**
  - `MinesweeperGame.razor` - Main game container
  - `GameBoard.razor` - Game board renderer
  - `GameStats.razor` - Game statistics header
  - `GameHeader.razor` - Page header
  - `GameFooter.razor` - Page footer
  - `DifficultySelector.razor` - Difficulty selection
  - `AchievementPanel.razor` - Achievement panel
  - `HelpModal.razor` - Help dialog

---

## Best Practices

### 1. Use Semantic Variants
Choose the appropriate variant based on the component's purpose:
```razor
<!-- Primary actions -->
<Card Variant="CardVariant.Primary">Main content</Card>

<!-- Secondary/supporting content -->
<Card Variant="CardVariant.Secondary">Supporting info</Card>

<!-- Glassmorphism effects -->
<Card Variant="CardVariant.Glass">Overlay content</Card>
```

### 2. Consistent Spacing
Use the Stack and Grid components for consistent spacing:
```razor
<!-- Good: Consistent gaps -->
<Stack Direction="Stack.StackDirection.Column" Gap="Stack.StackGap.Medium">
    <div>Item 1</div>
    <div>Item 2</div>
</Stack>

<!-- Avoid: Manual spacing -->
<div class="mb-4">Item 1</div>
<div class="mb-4">Item 2</div>
```

### 3. Responsive Layouts
Use Grid and Container for responsive designs:
```razor
<Container MaxWidth="Container.ContainerSize.SevenXL">
    <Grid Columns="1" TabletColumns="2" DesktopColumns="3">
        <!-- Automatically responsive -->
    </Grid>
</Container>
```

### 4. Accessibility
All components include proper ARIA attributes and keyboard navigation support. When using custom content, maintain accessibility:
```razor
<Button AriaLabel="Close modal">✕</Button>
<NavItem Href="stats" Label="Statistics" /> <!-- Label for screen readers -->
```

---

## Migration Guide

### Before (Inline Tailwind)
```razor
<div class="mx-auto px-4 py-6 max-w-7xl">
    <div class="bg-gray-800 rounded-2xl shadow-2xl p-5">
        <h1 class="text-3xl font-bold text-white flex items-center gap-3">
            <span class="text-4xl">📊</span>
            Statistics
        </h1>
        <div class="grid md:grid-cols-3 gap-4">
            <!-- Content -->
        </div>
    </div>
</div>
```

### After (UI Kit Components)
```razor
<Container>
    <Card Variant="CardVariant.Primary">
        <SectionHeader Icon="📊" Title="Statistics" Size="SectionHeader.HeaderSize.Large" />
        <Grid Columns="1" TabletColumns="3" Gap="Grid.GridGap.Medium">
            <!-- Content -->
        </Grid>
    </Card>
</Container>
```

### Benefits
- **50% less markup** - Reduced code repetition
- **Consistency** - Single source of truth for styling
- **Maintainability** - Easy to update styles globally
- **Type safety** - Enum-based props prevent errors
- **Accessibility** - Built-in ARIA support

---

## Support

For questions or issues with the UI Kit components, please refer to:
- Component source code in `MineSweeper/Components/`
- This documentation file
- Existing usage examples in `Statistics.razor`, `Achievements.razor`, `GameLayout.razor`, and `HelpModal.razor`
