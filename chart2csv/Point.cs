using System;
using System.Collections.Generic;
using System.Linq;

namespace chart2csv
{
    public readonly struct Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Point AverageFromPixels(HashSet<Pixel> pixels) => new(
            pixels.Average(x => x.X),
            pixels.Average(x => x.Y)
        );

        public double TranslateYToValue(int totalPixels, int startPixel, int startValue)
        {
            return Math.Pow(10, (Y - startPixel) / totalPixels) * startValue;
        }
    }
}
