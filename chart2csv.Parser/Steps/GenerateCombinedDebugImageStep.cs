using chart2csv.Parser.States;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace chart2csv.Parser.Steps;

public class GenerateCombinedDebugImageStep : ParserStep<AllDebugImagesState, CombinedDebugImageState>
{
    public override CombinedDebugImageState Process(AllDebugImagesState input)
    {
        var inputImage = input.ChartLayoutImageState.ChartLayoutState.ChartDimensionsState
            .ChartOriginState.InitialState.InputImage;

        var combinedOverlay = new Image<Rgba32>(inputImage.Width, inputImage.Height);
        combinedOverlay.Mutate(context =>
        {
            context.DrawImage(input.ChartLayoutImageState.ChartLayoutOverlay, 1);
            context.DrawImage(input.LineOverlayChartState.LineOverlay, 1);
        });

        var combinedImage = new Image<Rgba32>(inputImage.Width, inputImage.Height, Color.White);
        combinedImage.Mutate(context => context.DrawImage(combinedOverlay, 1));

        return new CombinedDebugImageState(input, combinedOverlay, combinedImage);
    }
}
