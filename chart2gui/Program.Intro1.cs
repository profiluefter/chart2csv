using SFML.Graphics;

namespace chart2gui;

internal static partial class Program
{
    private static Sprite _intro1Sprite = null!;

    private static void InitializeIntro1()
    {
        _intro1Sprite = new Sprite(new Texture(new Image("og-gfx/by.jpg")));
    }

    private static void ExecuteIntro1()
    {
        _window.Draw(_intro1Sprite);
    }
}
