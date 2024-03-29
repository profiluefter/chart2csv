using chart2csv.Parser.States;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv.Parser.Steps;

public class GetPointsStep : ParserStep<InitialState, ChartWithPointsState>
{
    private static readonly Color PointColor = Color.ParseHex("3366CC");
    
    public override ChartWithPointsState Process(InitialState input)
    {
        var image = input.InputImage;
        
        if (image.Width != 1280 || image.Height != 720)
            // ReSharper disable once LogMessageIsSentenceProblem
            Log.Warning("Input image is not 1280x720 but instead {Width}x{Height}. This is uncharted territory.",
                image.Width, image.Height);
        
        var matchingPixels = GetPixelsWithAdjacents(image, PointColor);
        var rawPixelGroups = GetGroupsOfPixels(matchingPixels);
        var averagedPixelGroups = rawPixelGroups.Select(pixels => new Point(
                pixels.Average(x => x.X),
                pixels.Average(x => x.Y)
            ))
            .ToHashSet();
        
        if(averagedPixelGroups.Count == 0)
            throw new ParserException("No points found in image.");
        
        Log.Debug("Generated {GroupCount} points/pixel groups from {PixelCount} matching pixels", averagedPixelGroups.Count, matchingPixels.Count);

        return new ChartWithPointsState(input, matchingPixels, rawPixelGroups, averagedPixelGroups);
    }
    
     /**
     * Searches an image for plus-shaped or square patterns where all pixels have the specified color.
     */
    private static HashSet<Pixel> GetPixelsWithAdjacents(Image<Rgba32> image, Color hex)
    {
        var pixels = new HashSet<Pixel>();
        for (var x = 1; x < image.Width - 1; x++)
        for (var y = 1; y < image.Height - 1; y++)
        {
            if (
                (Color) image[x, y] == hex &&
                (Color) image[x - 1, y] == hex &&
                (Color) image[x + 1, y] == hex &&
                (Color) image[x, y - 1] == hex &&
                (Color) image[x, y + 1] == hex
            )
            {
                pixels.Add(new Pixel(x, y));
            }

            if (
                (Color) image[x, y] == hex &&
                (Color) image[x + 1, y] == hex &&
                (Color) image[x, y + 1] == hex &&
                (Color) image[x + 1, y + 1] == hex
            )
            {
                pixels.Add(new Pixel(x, y));
                pixels.Add(new Pixel(x + 1, y));
                pixels.Add(new Pixel(x, y + 1));
                pixels.Add(new Pixel(x + 1, y + 1));
            }
        }

        return pixels;
    }

    private static HashSet<HashSet<Pixel>> GetGroupsOfPixels(HashSet<Pixel> pixels)
    {
        var groupsOfPixels = new HashSet<HashSet<Pixel>>();
        var usedPixels = new HashSet<Pixel>();

        foreach (var groupOfPixels in pixels
                     .Where(x => !usedPixels.Contains(x))
                     .Select(x => GetGroupOfPixels(pixels, x)))
        {
            usedPixels.UnionWith(groupOfPixels);
            groupsOfPixels.Add(groupOfPixels);
        }

        return groupsOfPixels;
    }

    private static HashSet<Pixel> GetGroupOfPixels(HashSet<Pixel> pixels, Pixel pixel, HashSet<Pixel>? exclude = null)
    {
        var group = new HashSet<Pixel> {pixel};
        exclude ??= new HashSet<Pixel>();
        exclude.Add(pixel);

        for (var x = -1; x <= 1; x++)
        for (var y = -1; y <= 1; y++)
        {
            if (x == 0 && y == 0) continue;
            var adjacent = new Pixel(pixel.X + x, pixel.Y + y);
            if (pixels.Contains(adjacent) && !exclude.Contains(adjacent))
                group.Add(adjacent);
        }

        return group;
    }
}
