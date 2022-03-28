using System.Text.Json.Serialization;
using chart2csv.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv.Parser.States;

public class InitialState : ParserState
{
    [JsonConstructor]
    public InitialState(Image<Rgba32> inputImage) => InputImage = inputImage;

    public InitialState(string inputImagePath) => InputImage = Image.Load<Rgba32>(inputImagePath);

    [JsonConverter(typeof(Base64ImageJsonConverter))]
    public Image<Rgba32> InputImage { get; }
}
