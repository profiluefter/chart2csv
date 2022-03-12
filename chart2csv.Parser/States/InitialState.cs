using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv.Parser.States;

public class InitialState : ParserState
{
    public InitialState(Image<Rgba32> inputImage) => InputImage = inputImage;
    public InitialState(string inputImagePath) => InputImage = Image.Load<Rgba32>(inputImagePath);

    protected InitialState(InitialState input) : this(input.InputImage)
    {
    }

    public Image<Rgba32> InputImage { get; }
}
