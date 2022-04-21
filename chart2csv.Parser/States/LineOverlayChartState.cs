using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv.Parser.States;

public class LineOverlayChartState : ParserState
{
    public LineOverlayChartState(MergedChartState mergedChartState, Image<Rgba32> lineOverlayChart,
        Image<Rgba32> overlay)
    {
        MergedChartState = mergedChartState;
        LineOverlayChart = lineOverlayChart;
        LineOverlay = overlay;
    }

    public MergedChartState MergedChartState { get; }
    public Image<Rgba32> LineOverlay { get; }
    public Image<Rgba32> LineOverlayChart { get; }
}
