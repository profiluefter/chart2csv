using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

var window = new RenderWindow(new VideoMode(1280, 720), "weird flex" /*, Styles.Fullscreen*/);

window.Closed += (_, _) => window.Close();

var introMusic = new Music("og-sfx/intro.ogg");

var chartImage = new Image("charts/00.0-08.0-35.0-35.0-40.0-30.0-01.0-04.0-02.0-NONE.png");
var chartSprite = new Sprite(new Texture(chartImage));

introMusic.Play();

while (window.IsOpen)
{
    window.DispatchEvents();
    window.Clear();
    
    if (introMusic.PlayingOffset > Time.FromSeconds(6))
        window.Draw(chartSprite);
    
    window.Display();
}
