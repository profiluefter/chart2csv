using chart2csv.Parser.States;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace chart2csv.Parser.Steps;

public class GenerateLineOverlayStep : ParserStep<MergedChartState, LineOverlayChartState>
{
    public override LineOverlayChartState Process(MergedChartState input)
    {
        var inputImage = input.ChartWithPointsState.InitialState.InputImage;

        var overlay = new Image<Rgba32>(
            inputImage.Width,
            inputImage.Height,
            new Rgba32(255, 255, 255, 0));

        var points = input.Points.Select(x => new PointF((float)x.X, (float)x.Y)).ToArray();
        overlay.Mutate(x => x.DrawLines(Color.Red, 1.0f, points));

        var image = inputImage.Clone();
        image.Mutate(x => x.DrawImage(overlay, 1));

        return new LineOverlayChartState(input, image, overlay);
    }
}
