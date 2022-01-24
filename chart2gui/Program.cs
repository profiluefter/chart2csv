using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace chart2gui;

internal static partial class Program
{
    private static RenderWindow _window = null!;
    private static GameState _gameState = GameState.Intro1;

    public static void Main()
    {
        InitializeIntro1();
        InitializeIntro2();
        InitializeMenu();

        var cursorImage = new Image("og-gfx/hand.bmp");
        cursorImage.CreateMaskFromColor(new Color(255, 0, 255));
        var cursorSprite = new Sprite(new Texture(cursorImage));

        var introMusic = new Music("og-sfx/intro.ogg");
        introMusic.Play();

        _window = new RenderWindow(new VideoMode(1280, 720), "weird flex", Styles.Fullscreen);
        _window.SetMouseCursorVisible(false);

        _window.Closed += (_, _) => _window.Close();

        while (_window.IsOpen)
        {
            _window.DispatchEvents();
            _window.Clear();

            if (introMusic.PlayingOffset > Time.FromSeconds(10.5f))
                _gameState = GameState.Menu;
            else if (introMusic.PlayingOffset > Time.FromSeconds(6))
                _gameState = GameState.Intro2;

            switch (_gameState)
            {
                case GameState.Intro1:
                    ExecuteIntro1();
                    break;
                case GameState.Intro2:
                    ExecuteIntro2();
                    break;
                case GameState.Menu:
                    ExecuteMenu();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_gameState));
            }

            cursorSprite.Position = (Vector2f)Mouse.GetPosition(_window);
            _window.Draw(cursorSprite);

            _window.Display();
        }
    }
}
