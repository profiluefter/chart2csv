using System.Text.Json;
using System.Text.Json.Serialization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv.Utils;

public class Base64ImageJsonConverter : JsonConverter<Image<Rgba32>>
{
    public override Image<Rgba32>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        Image.Load<Rgba32>(Convert.FromBase64String(reader.GetString()!));

    public override void Write(Utf8JsonWriter writer, Image<Rgba32> value, JsonSerializerOptions options)
    {
        var stream = new MemoryStream();
        value.SaveAsPng(stream);
        writer.WriteStringValue(Convert.ToBase64String(stream.ToArray()));
    }
}
