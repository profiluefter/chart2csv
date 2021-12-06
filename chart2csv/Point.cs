using System;
using System.Collections.Generic;

namespace chart2csv
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
            
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Point AverageFromPixels(List<Pixel> pixels)
        {
            var xGes = 0.0;
            var yGes = 0.0;
                
            pixels.ForEach(x =>
            {
                xGes += x.X;
                yGes += x.Y;
            });

            return new Point(xGes/pixels.Count, yGes/pixels.Count);
        }

        public double TranslateYToValue(int totalPixels, int startPixel, int startValue)
        {
            return Math.Pow(10, (Y-startPixel)/totalPixels) * startValue;
        }
    }
}