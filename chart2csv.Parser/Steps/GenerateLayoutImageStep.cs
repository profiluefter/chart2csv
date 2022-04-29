using chart2csv.Parser.States;
using Serilog;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace chart2csv.Parser.Steps;

public class GenerateLayoutImageStep : ParserStep<ChartLayoutState, ChartLayoutImageState>
{
    private static readonly string[] DefaultFontNames = {
        "Arial",
        "Times New Roman",
        "Verdana",
        "Courier New",
        "Comic Sans MS"
    };

    public override ChartLayoutImageState Process(ChartLayoutState input)
    {
        var inputImage = input.ChartDimensionsState.ChartOriginState.InitialState.InputImage;
        var overlay = new Image<Rgba32>(inputImage.Width, inputImage.Height);

        var originPoint = input.ChartDimensionsState.ChartOriginState.OriginPoint;
        overlay[originPoint.X, originPoint.Y] = Color.Red;

        var font = DefaultFontNames
            .Select(x => !SystemFonts
                .TryGet(x, out var font1)
                ? null
                : font1.CreateFont(10))
            .FirstOrDefault(x => x is not null);

        if (font is not null)
            Log.Debug("Using font {Font}", font.Name);

        if (font is null)
        {
            Log.Warning("None of the default fonts are available on this system");
            var fontFamilies = SystemFonts.Families.ToList();
            font = fontFamilies[Random.Shared.Next(0, fontFamilies.Count)].CreateFont(10);
            Log.Warning("Selecting random font: {Font}", font.Name);
        }

        var digits = input.YAxisState.YAxisNumbers;
        overlay.Mutate(context =>
        {
            foreach (var (yPosition, value) in digits)
            {
                context.DrawLines(
                    Color.Green, 1f,
                    new PointF(originPoint.X, yPosition), new PointF(0, yPosition)
                );
                context.DrawText(value.ToString(), font, Color.Blue, new PointF(0, yPosition));
            }
        });

        var chartHeight = input.ChartDimensionsState.ChartHeight;
        var chartWidth = input.ChartDimensionsState.ChartWidth;

        overlay.Mutate(context =>
        {
            context.DrawPolygon(Color.Red, 1,
                new PointF(originPoint.X, originPoint.Y),
                new PointF(originPoint.X + chartWidth, originPoint.Y),
                new PointF(originPoint.X + chartWidth, originPoint.Y - chartHeight),
                new PointF(originPoint.X, originPoint.Y - chartHeight));
        });

        var image = inputImage.Clone();
        image.Mutate(x => x.DrawImage(overlay, 1));

        return new ChartLayoutImageState(input, overlay, image);
    }
}
