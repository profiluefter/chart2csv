using chart2csv.Parser.States;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace chart2csv.Parser.Steps;

public class GenerateClusterImageStep : ParserStep<ChartWithPointsState, PointClusterImageState>
{
    public override PointClusterImageState Process(ChartWithPointsState input)
    {
        var inputImage = input.InitialState.InputImage;

        var overlay = new Image<Rgba32>(
            inputImage.Width,
            inputImage.Height,
            new Rgba32(255, 255, 255, 0));

        foreach (var group in input.RawPixelGroups)
        {
            var hash = group.GetHashCode();
            var color = Color.FromRgb(
                    (byte)(hash & 0xFF),
                    (byte)((hash >> 8) & 0xFF),
                    (byte)((hash >> 16) & 0xFF));
            foreach (var pixel in group) 
                overlay[pixel.X, pixel.Y] = color;
        }

        var image = inputImage.Clone();
        image.Mutate(x => x.DrawImage(overlay, 1));

        return new PointClusterImageState(input, overlay, image);
    }
}
