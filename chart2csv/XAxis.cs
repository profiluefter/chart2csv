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
    private static readonly DateTime StartDate = new(2018, 01, 01, 23, 00, 00);
    private static readonly DateTime EndDate = new(2021, 02, 01, 06, 00, 00);
    
    private readonly int _start;
    private readonly int _end;

    private XAxis(int xStart, int xEnd)
    {
        _start = xStart;
        _end = xEnd;
    }

    public static XAxis DetectXAxis(Point first, Point last)
    {
        return new XAxis((int)first.X, (int)last.X);
    }

    /**
     * Gets the date value for a given x coordinate (top = 0).
     */
    public DateTime GetValue(double x) // TODO: check if the data not accurate or there is an error here
    {
        var startValue = new DateTimeOffset(StartDate).ToUnixTimeSeconds();
        var endValue = new DateTimeOffset(EndDate).ToUnixTimeSeconds();
        var valueRange = endValue - startValue;

        var range = _end - _start;
        var relativePosition = x - _start;

        var percent = relativePosition / range;

        var value = percent * valueRange + startValue;
        return DateTimeOffset.FromUnixTimeSeconds((long) value).DateTime;
    }
}
