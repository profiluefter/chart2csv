using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv;

/**
 * Value axis
 */
public class YAxis
{
    public static YAxis DetectYAxis(Image<Rgba32> image)
    {
        throw new NotImplementedException();
    }

    /**
     * Gets the value for a given y coordinate (left = 0). 
     */
    public double GetValue(double y)
    {
        throw new NotImplementedException();
    }
}
