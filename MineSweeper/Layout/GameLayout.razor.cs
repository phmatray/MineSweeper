namespace MineSweeper.Layout;

public partial class GameLayout
{
    private bool _showMobileMenu = false;

    private void ToggleMobileMenu()
    {
        _showMobileMenu = !_showMobileMenu;
    }

    private void CloseMobileMenu()
    {
        _showMobileMenu = false;
    }
}