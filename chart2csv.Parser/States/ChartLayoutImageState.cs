using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv.Parser.States;

public class ChartLayoutImageState : ParserState
{
    public ChartLayoutImageState(ChartLayoutState chartLayoutState,
        Image<Rgba32> chartLayoutOverlay, Image<Rgba32> chartLayoutImage)
    {
        ChartLayoutState = chartLayoutState;
        ChartLayoutOverlay = chartLayoutOverlay;
        ChartLayoutImage = chartLayoutImage;
    }

    public ChartLayoutState ChartLayoutState { get; }
    public Image<Rgba32> ChartLayoutOverlay { get; }
    public Image<Rgba32> ChartLayoutImage { get; }
}
