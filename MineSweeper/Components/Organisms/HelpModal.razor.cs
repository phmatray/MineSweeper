using Microsoft.AspNetCore.Components;

namespace MineSweeper.Components.Organisms;

public partial class HelpModal
{
    [Parameter] public bool IsVisible { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }

    private string GetNumberColorClass(int number)
    {
        return number switch
        {
            1 => "text-blue-400",
            2 => "text-green-400",
            3 => "text-red-400",
            4 => "text-purple-400",
            5 => "text-yellow-400",
            6 => "text-pink-400",
            7 => "text-gray-300",
            8 => "text-gray-100",
            _ => "text-gray-400"
        };
    }
}