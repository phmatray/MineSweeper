using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MineSweeper.UIKit.Components.Molecules;

public partial class GameCell
{
    [Parameter] public int Row { get; set; }
    [Parameter] public int Col { get; set; }
    [Parameter] public bool IsRevealed { get; set; }
    [Parameter] public bool IsFlagged { get; set; }
    [Parameter] public bool IsMine { get; set; }
    [Parameter] public int AdjacentMines { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnRightClick { get; set; }
    [Parameter] public EventCallback OnDoubleClick { get; set; }

    private string GetCellClass()
    {
        if (!IsRevealed)
        {
            return "cell-unrevealed";
        }

        if (IsMine)
        {
            return "cell-mine";
        }

        var numberClass = AdjacentMines > 0 ? $" mine-count-{AdjacentMines}" : "";
        return $"cell-revealed{numberClass}";
    }

    private string GetCellContent()
    {
        if (!IsRevealed && IsFlagged)
        {
            return "🚩";
        }

        if (!IsRevealed)
        {
            return "";
        }

        if (IsMine)
        {
            return "💣";
        }

        return AdjacentMines > 0 ? AdjacentMines.ToString() : "";
    }
}