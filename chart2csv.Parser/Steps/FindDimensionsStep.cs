using chart2csv.Parser.States;
using Serilog;
using SixLabors.ImageSharp;

namespace chart2csv.Parser.Steps;

public class FindDimensionsStep : ParserStep<ChartOriginState, ChartDimensionsState>
{
    private static readonly List<Color> LineColors = new[]
    {
        "C0C0C0", // line color
        "A8A8A8", // x label color and y edge case
        "D0D0D0", // x end color
        "B1B1B1", // y marker color
        "B2B2B2", // y label color
        "CFCFCF", // y end color
        "BDBDBD" // y edge case
    }.Select(Color.ParseHex).ToList();

    public override ChartDimensionsState Process(ChartOriginState input)
    {
        var image = input.InitialState.InputImage;
        var origin = input.OriginPoint;

        var chartWidth = 1;
        while (LineColors.Contains(image[origin.X + chartWidth, origin.Y]))
            chartWidth++;

        var chartHeight = 1;
        while (LineColors.Contains(image[origin.X, origin.Y - chartHeight]))
            chartHeight++;

        // Compensate for origin point
        chartWidth--;
        chartHeight--;

        Log.Debug("Found chart dimensions: {Width}x{Height}", chartWidth, chartHeight);
        
        return new ChartDimensionsState(input, chartWidth, chartHeight);
    }
}
