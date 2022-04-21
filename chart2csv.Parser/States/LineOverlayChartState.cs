using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv.Parser.States;

public class LineOverlayChartState : ParserState
{
    public LineOverlayChartState(MergedChartState mergedChartState,
        Image<Rgba32> overlay, Image<Rgba32> overlayChart)
    {
        MergedChartState = mergedChartState;
        LineOverlayChart = overlayChart;
        LineOverlay = overlay;
    }

    public MergedChartState MergedChartState { get; }
    public Image<Rgba32> LineOverlay { get; }
    public Image<Rgba32> LineOverlayChart { get; }
}
