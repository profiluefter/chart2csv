using System;

namespace chart2csv;

public readonly struct Point
{
    public double X { get; }
    public double Y { get; }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double TranslateYToValue(int totalPixels, int startPixel, int startValue)
    {
        return Math.Pow(10, (Y - startPixel) / totalPixels) * startValue;
    }
}
