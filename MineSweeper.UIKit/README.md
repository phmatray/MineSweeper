# MineSweeper.UIKit

A reusable Razor Class Library (RCL) containing UI components built with Blazor and styled with Tailwind CSS. This library follows atomic design principles to provide a consistent, maintainable, and accessible component system.

## 🎨 Features

- **Atomic Design Architecture** - Components organized as Atoms and Molecules
- **Type-Safe Props** - Enum-based variants prevent errors
- **Accessibility First** - Built-in ARIA support and keyboard navigation
- **Tailwind CSS** - Utility-first styling approach
- **Fully Documented** - Comprehensive prop documentation
- **Blazor Native** - Pure Blazor components with no JavaScript dependencies

## 📦 Installation

### Add Project Reference

```bash
dotnet add reference path/to/MineSweeper.UIKit/MineSweeper.UIKit.csproj
```

### Update _Imports.razor

Add the following to your project's `_Imports.razor`:

```razor
@using MineSweeper.UIKit.Components.Atoms
@using MineSweeper.UIKit.Components.Molecules
```

## 🧱 Component Catalog

### Atoms (Basic Building Blocks)

| Component | Description |
|-----------|-------------|
| **Badge** | Small label or tag component |
| **Button** | Interactive button with multiple variants |
| **Card** | Versatile container with variants and padding options |
| **CardHeader** | Styled header section for cards |
| **CardBody** | Main content area for cards |
| **CardFooter** | Footer section for cards |
| **Container** | Responsive container with max-width constraints |
| **Grid** | CSS Grid layout with responsive columns |
| **Icon** | Icon wrapper component |
| **NavItem** | Navigation link with consistent styling |
| **NumberDisplay** | Styled number display component |
| **SectionHeader** | Heading component with optional icon |
| **Stack** | Flexbox layout for arranging children |

### Molecules (Component Combinations)

| Component | Description |
|-----------|-------------|
| **AchievementCard** | Display card for achievements |
| **DifficultyButton** | Button for selecting game difficulty |
| **GameCell** | Minesweeper game cell component |
| **Modal** | Dialog/modal with animations and accessibility |
| **ProgressBar** | Visual progress indicator |
| **StatCard** | Statistics display card |
| **Toast** | Notification component with auto-dismiss |

## 🚀 Quick Start

### Example 1: Simple Card Layout

```razor
<Container>
    <Card Variant="CardVariant.Primary" Padding="CardPadding.Medium">
        <CardHeader>
            <SectionHeader Icon="🎮" Title="Welcome" Size="SectionHeader.HeaderSize.Large" />
        </CardHeader>
        <CardBody>
            <p>This is a simple card layout using the UI Kit.</p>
        </CardBody>
        <CardFooter>
            <Button Variant="ButtonVariant.Primary">Get Started</Button>
        </CardFooter>
    </Card>
</Container>
```

### Example 2: Responsive Grid

```razor
<Container>
    <Grid Columns="1" TabletColumns="2" DesktopColumns="3" Gap="Grid.GridGap.Medium">
        <Card Variant="CardVariant.Secondary">
            <CardBody>Item 1</CardBody>
        </Card>
        <Card Variant="CardVariant.Secondary">
            <CardBody>Item 2</CardBody>
        </Card>
        <Card Variant="CardVariant.Secondary">
            <CardBody>Item 3</CardBody>
        </Card>
    </Grid>
</Container>
```

### Example 3: Modal Dialog

```razor
<Button OnClick="@(() => _showModal = true)">Open Modal</Button>

<Modal IsVisible="@_showModal"
       OnClose="@(() => _showModal = false)"
       Title="Confirmation"
       Icon="⚠️"
       Size="Modal.ModalSize.Medium">
    <p>Are you sure you want to proceed?</p>
    <div class="mt-4 flex gap-2 justify-end">
        <Button Variant="ButtonVariant.Secondary" OnClick="@(() => _showModal = false)">
            Cancel
        </Button>
        <Button Variant="ButtonVariant.Primary" OnClick="@HandleConfirm">
            Confirm
        </Button>
    </div>
</Modal>

@code {
    private bool _showModal = false;

    private void HandleConfirm()
    {
        _showModal = false;
        // Handle confirmation
    }
}
```

### Example 4: Stack Layout

```razor
<Stack Direction="Stack.StackDirection.Column" Gap="Stack.StackGap.Medium">
    <Stack Direction="Stack.StackDirection.Row" Justify="Stack.StackJustify.Between">
        <span>Total Games</span>
        <span class="font-bold">42</span>
    </Stack>
    <Stack Direction="Stack.StackDirection.Row" Justify="Stack.StackJustify.Between">
        <span>Wins</span>
        <span class="font-bold text-green-400">28</span>
    </Stack>
    <Stack Direction="Stack.StackDirection.Row" Justify="Stack.StackJustify.Between">
        <span>Win Rate</span>
        <span class="font-bold text-blue-400">66.7%</span>
    </Stack>
</Stack>
```

## 📚 Component Reference

### Button

```razor
<Button Variant="ButtonVariant.Primary"
        OnClick="@HandleClick"
        Disabled="false"
        AriaLabel="Click me">
    Click Me
</Button>
```

**Props:**
- `Variant` - ButtonVariant (Primary | Secondary | Success | Danger | Warning | Icon | Outline | Ghost)
- `OnClick` - EventCallback<MouseEventArgs>
- `Disabled` - bool
- `Type` - string (default: "button")
- `AriaLabel` - string
- `AdditionalClasses` - string

### Card

```razor
<Card Variant="CardVariant.Primary"
      Padding="CardPadding.Medium"
      Hover="true"
      Border="false">
    <!-- Content -->
</Card>
```

**Props:**
- `Variant` - CardVariant (Primary | Secondary | Glass | GlassStrong | Elevated | Dark | Outline)
- `Padding` - CardPadding (None | Small | Medium | Large)
- `Hover` - bool (enable hover effect)
- `Border` - bool (add border)
- `Class` - string (additional CSS classes)

### Container

```razor
<Container MaxWidth="Container.ContainerSize.SevenXL"
           Padding="Container.ContainerPadding.Medium"
           CenterContent="true">
    <!-- Content -->
</Container>
```

**Props:**
- `MaxWidth` - ContainerSize (Small | Medium | Large | ExtraLarge | TwoXL | ThreeXL | FourXL | FiveXL | SixXL | SevenXL | Full)
- `Padding` - ContainerPadding (None | Small | Medium | Large)
- `CenterContent` - bool (center horizontally)

### Stack

```razor
<Stack Direction="Stack.StackDirection.Row"
       Align="Stack.StackAlign.Center"
       Justify="Stack.StackJustify.Between"
       Gap="Stack.StackGap.Medium"
       Wrap="false">
    <!-- Children -->
</Stack>
```

**Props:**
- `Direction` - StackDirection (Row | RowReverse | Column | ColumnReverse)
- `Align` - StackAlign (Start | Center | End | Stretch | Baseline)
- `Justify` - StackJustify (Start | Center | End | Between | Around | Evenly)
- `Gap` - StackGap (None | ExtraSmall | Small | Medium | Large | ExtraLarge | TwoXL)
- `Wrap` - bool

### Grid

```razor
<Grid Columns="1"
      TabletColumns="2"
      DesktopColumns="3"
      Gap="Grid.GridGap.Medium">
    <!-- Children -->
</Grid>
```

**Props:**
- `Columns` - int (mobile columns)
- `TabletColumns` - int? (tablet breakpoint)
- `DesktopColumns` - int? (desktop breakpoint)
- `Gap` - GridGap (None | ExtraSmall | Small | Medium | Large | ExtraLarge)

### Modal

```razor
<Modal IsVisible="@_visible"
       OnClose="@HandleClose"
       Title="Modal Title"
       Icon="📝"
       Size="Modal.ModalSize.Medium"
       ShowHeader="true"
       Dismissible="true"
       CloseOnOverlayClick="true">
    <!-- Modal content -->
</Modal>
```

**Props:**
- `IsVisible` - bool (required)
- `OnClose` - EventCallback (required)
- `Title` - string
- `Icon` - string
- `Size` - ModalSize (Small | Medium | Large | ExtraLarge | Full)
- `ShowHeader` - bool
- `Dismissible` - bool
- `CloseOnOverlayClick` - bool
- `HeaderContent` - RenderFragment
- `FooterContent` - RenderFragment

### Toast

```razor
<Toast IsVisible="@_showToast"
       Title="Success!"
       Message="Operation completed successfully"
       Icon="✅"
       Type="Toast.ToastType.Success"
       AutoDismissMs="5000"
       Dismissible="true"
       OnDismiss="@HandleDismiss" />
```

**Props:**
- `IsVisible` - bool
- `Title` - string
- `Message` - string
- `Icon` - string
- `Type` - ToastType (Success | Error | Warning | Info)
- `AutoDismissMs` - int? (auto-dismiss after milliseconds)
- `Dismissible` - bool
- `OnDismiss` - EventCallback

## 🎨 Styling

This library uses Tailwind CSS utility classes. Ensure your consuming project has Tailwind CSS configured and includes the necessary styles.

### Required CSS Classes

The components use standard Tailwind utility classes. No additional CSS compilation is required from the RCL itself, as Blazor RCLs can include scoped CSS files.

## ♿ Accessibility

All components are built with accessibility in mind:

- Proper ARIA labels and attributes
- Keyboard navigation support
- Focus management
- Screen reader friendly
- Semantic HTML elements

## 📖 Full Documentation

For comprehensive documentation with examples, see the [UI-KIT.md](../UI-KIT.md) file in the repository root.

## 🤝 Usage in MineSweeper

This library is used extensively in the MineSweeper Blazor application. See real-world examples in:

- `MineSweeper/Pages/Statistics.razor`
- `MineSweeper/Pages/Achievements.razor`
- `MineSweeper/Layout/GameLayout.razor`
- `MineSweeper/Components/Organisms/HelpModal.razor`

## 📝 License

This library is part of the MineSweeper project.

## 🔧 Development

### Building the Library

```bash
dotnet build MineSweeper.UIKit/MineSweeper.UIKit.csproj
```

### Project Structure

```
MineSweeper.UIKit/
├── Components/
│   ├── Atoms/          # Basic building blocks (15 components)
│   │   ├── Button.razor
│   │   ├── Card.razor
│   │   ├── Container.razor
│   │   └── ...
│   └── Molecules/      # Composite components (7 components)
│       ├── Modal.razor
│       ├── Toast.razor
│       └── ...
├── wwwroot/            # Static assets
├── _Imports.razor      # Global imports
└── MineSweeper.UIKit.csproj
```

## 🚀 Future Enhancements

Potential additions to the library:

- Form components (Input, Select, Checkbox, etc.)
- Data display components (Table, List, etc.)
- Feedback components (Spinner, Skeleton, Alert, etc.)
- Navigation components (Breadcrumb, Tabs, Pagination, etc.)
- Overlay components (Popover, Tooltip, Dropdown, etc.)

---

**Built with ❤️ using Blazor and Tailwind CSS**
