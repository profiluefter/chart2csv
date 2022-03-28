using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv.Parser.States;

public class LineOverlayChartState : ParserState
{
    public LineOverlayChartState(MergedChartState mergedChartState, Image<Rgba32> lineOverlayChart)
    {
        MergedChartState = mergedChartState;
        LineOverlayChart = lineOverlayChart;
    }

    public MergedChartState MergedChartState { get; }
    public Image<Rgba32> LineOverlayChart { get; }
}
