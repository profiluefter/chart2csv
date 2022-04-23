using chart2csv.Parser.States;
using Serilog;
using SixLabors.ImageSharp;

namespace chart2csv.Parser.Steps;

public class FindOriginStep : ParserStep<InitialState, ChartOriginState>
{
    private static readonly Color LineCornerColor = Color.ParseHex("B2B2B2");
    private static readonly Color LineColor = Color.ParseHex("C0C0C0");
    private static readonly Color BackgroundColor = Color.ParseHex("FFFFFF");
    
    public override ChartOriginState Process(InitialState input)
    {
        Pixel? origin = null;
        var image = input.InputImage;

        for (var i = 0; i < image.Width; i++)
        for (var j = 0; j < image.Height; j++)
        {
            if ((Color)image[i, j] != LineCornerColor ||
                (Color)image[i, j - 1] != LineColor ||
                (Color)image[i, j + 1] != BackgroundColor ||
                (Color)image[i + 1, j] != LineColor ||
                (Color)image[i - 1, j] != BackgroundColor) continue;

            if (origin == null)
                origin = new Pixel(i, j);
            else
                throw new Exception("Multiple origin points found");
        }

        if (origin == null)
            throw new Exception("No origin point found");
        
        Log.Debug("Found chart origin point at {@Point}", origin.Value);
        return new ChartOriginState(input, origin.Value);
    }
}
