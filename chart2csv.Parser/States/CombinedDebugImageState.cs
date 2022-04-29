using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv.Parser.States;

public class CombinedDebugImageState : ParserState
{
    public AllDebugImagesState AllDebugImagesState { get; }
    public Image<Rgba32> CombinedDebugOverlay { get; }
    public Image<Rgba32> CombinedDebugImage { get; }

    public CombinedDebugImageState(AllDebugImagesState allDebugImagesState,
        Image<Rgba32> combinedDebugOverlay, Image<Rgba32> combinedDebugImage)
    {
        AllDebugImagesState = allDebugImagesState;
        CombinedDebugOverlay = combinedDebugOverlay;
        CombinedDebugImage = combinedDebugImage;
    }
}
