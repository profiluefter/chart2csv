using SFML.Graphics;

namespace chart2gui;

internal static partial class Program
{
    private static Sprite _intro2Sprite = null!;

    private static void InitializeIntro2()
    {
        _intro2Sprite = new Sprite(new Texture(new Image("og-gfx/lake.jpg")));
    }

    private static void ExecuteIntro2()
    {
        _window.Draw(_intro2Sprite);
    }
}
