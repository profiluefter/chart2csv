using chart2csv.Parser.States;
using Serilog;
using SixLabors.ImageSharp;

namespace chart2csv.Parser.Steps;

public class FindDimensionsStep : ParserStep<ChartOriginState, ChartDimensionsState>
{
    private static readonly Color LineColor = Color.ParseHex("C0C0C0");
    private static readonly Color LineXLabelColor = Color.ParseHex("A8A8A8");
    private static readonly Color LineXEndColor = Color.ParseHex("D0D0D0");
    private static readonly Color LineYMarkerColor = Color.ParseHex("B1B1B1");
    private static readonly Color LineYLabelColor = Color.ParseHex("B2B2B2");
    private static readonly Color LineYEndColor = Color.ParseHex("CFCFCF");
    
    public override ChartDimensionsState Process(ChartOriginState input)
    {
        var image = input.InitialState.InputImage;
        var origin = input.OriginPoint;
        
        var chartWidth = 1;
        while ((Color)image[origin.X + chartWidth, origin.Y] == LineColor ||
               (Color)image[origin.X + chartWidth, origin.Y] == LineXLabelColor ||
               (Color)image[origin.X + chartWidth, origin.Y] == LineXEndColor)
        {
            chartWidth++;
        }

        var chartHeight = 1;
        while ((Color)image[origin.X, origin.Y - chartHeight] == LineColor ||
               (Color)image[origin.X, origin.Y - chartHeight] == LineYMarkerColor ||
               (Color)image[origin.X, origin.Y - chartHeight] == LineYLabelColor ||
               (Color)image[origin.X, origin.Y - chartHeight] == LineYEndColor)
        {
            chartHeight++;
        }

        // Compensate for origin point
        chartWidth--;
        chartHeight--;

        Log.Debug("Found chart dimensions: {Width}x{Height}", chartWidth, chartHeight);
        
        return new ChartDimensionsState(input, chartWidth, chartHeight);
    }
}
