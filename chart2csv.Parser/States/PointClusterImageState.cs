using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv.Parser.States;

public class PointClusterImageState : ParserState
{
    public PointClusterImageState(ChartWithPointsState previousState, Image<Rgba32> overlay, Image<Rgba32> image)
    {
        ChartWithPointsState = previousState;
        PointClusterOverlay = overlay;
        PointClusterOverlayChart = image;
    }

    public ChartWithPointsState ChartWithPointsState { get; }
    public Image<Rgba32> PointClusterOverlay { get; }
    public Image<Rgba32> PointClusterOverlayChart { get; }
}
