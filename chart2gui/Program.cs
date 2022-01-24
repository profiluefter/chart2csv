using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

var window = new RenderWindow(new VideoMode(1280, 720), "weird flex" /*, Styles.Fullscreen*/);
window.SetMouseCursorVisible(false);

window.Closed += (_, _) => window.Close();

var intro1Image = new Image("og-gfx/by.jpg");
var intro1Sprite = new Sprite(new Texture(intro1Image));

var intro2Image = new Image("og-gfx/lake.jpg");
var intro2Sprite = new Sprite(new Texture(intro2Image));

var menuImage = new Image("og-gfx/titleback.jpg");
var menuSprite = new Sprite(new Texture(menuImage));

var chartImage = new Image("charts/00.0-08.0-35.0-35.0-40.0-30.0-01.0-04.0-02.0-NONE.png");
var chartSprite = new Sprite(new Texture(chartImage));

var cursorImage = new Image("og-gfx/hand.bmp");
cursorImage.CreateMaskFromColor(new Color(255,0,255));
var cursorSprite = new Sprite(new Texture(cursorImage));

var introMusic = new Music("og-sfx/intro.ogg");
introMusic.Play();

while (window.IsOpen)
{
    window.DispatchEvents();
    window.Clear();
    
    if (introMusic.PlayingOffset > Time.FromSeconds(10.5f))
        window.Draw(menuSprite);
    else if (introMusic.PlayingOffset > Time.FromSeconds(6))
        window.Draw(intro2Sprite);
    else
        window.Draw(intro1Sprite);

    cursorSprite.Position = (Vector2f)Mouse.GetPosition(window);
    window.Draw(cursorSprite);
    
    window.Display();
}
