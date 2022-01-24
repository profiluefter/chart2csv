using SFML.Graphics;

namespace chart2gui;

internal static partial class Program
{
    private static Sprite _menuSprite = null!;

    private static void InitializeMenu()
    {
        _menuSprite = new Sprite(new Texture(new Image("og-gfx/titleback.jpg")));
    }

    private static void ExecuteMenu()
    {
        _window.Draw(_menuSprite);
    }
}
