using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv;

/**
 * Date axis
 */
public class XAxis
{
    public static XAxis DetectXAxis(Image<Rgba32> image)
    {
        throw new NotImplementedException();
    }

    /**
     * Gets the value for a given x coordinate (top = 0).
     */
    public DateTime GetValue(double x)
    {
        throw new NotImplementedException();
    }
}
