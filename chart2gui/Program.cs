using SFML.Graphics;
using SFML.Window;

var window = new RenderWindow(new VideoMode(1280, 720), "Title ", Styles.Fullscreen);

window.Closed += (_, _) => window.Close();

var chartImage = new Image("charts/00.0-08.0-35.0-35.0-40.0-30.0-01.0-04.0-02.0-NONE.png");
var sprite = new Sprite(new Texture(chartImage));

while (window.IsOpen)
{
    window.DispatchEvents();
    window.Clear();
    window.Draw(sprite);
    window.Display();
}
