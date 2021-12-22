using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using static chart2csv.Constants;

namespace chart2csv;

/**
 * Date axis
 */
public class XAxis
{
    private readonly int _start;
    private readonly int _end;

    private XAxis(int xStart, int xEnd)
    {
        _start = xStart;
        _end = xEnd;
    }

    public static XAxis DetectXAxis(Image<Rgba32> image, Image<Rgba32> newImage, Pixel origin, (int, int) dimensions)
    {
        return new XAxis(origin.X, origin.X + dimensions.Item1);
    }

    /**
     * Gets the date value for a given x coordinate (top = 0).
     */
    public DateTime GetValue(double x)
    {
        throw new NotImplementedException();
    }
}
