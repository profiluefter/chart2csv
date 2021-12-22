using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using static chart2csv.Constants;

namespace chart2csv;

/**
 * Value axis
 */
public class YAxis
{
    private readonly Dictionary<int,int> _numbers;

    private YAxis(Dictionary<int,int> numbers)
    {
        _numbers = numbers;
    }

    public static YAxis DetectYAxis(Image<Rgba32> image, Image<Rgba32> newImage, Pixel origin, (int, int) dimensions)
    {
        var digits = new Dictionary<int, int>();

        for (var x = 2; x < origin.X; x++)
        for (var y = 0; y < origin.Y; y++)
        {
            if ((Color) image[x - 2, y] != NumberZero || (Color) image[x + 2, y] != NumberZero) continue;
            newImage[x, y] = Color.Black;

            if (!digits.ContainsKey(y))
                digits[y] = 0;
            digits[y]++;
        }

        var numbers = new Dictionary<int, int>();

        foreach (var (y, count) in digits)
        {
            var realY = y;
            if ((Color) image[origin.X, y] == LineYLabelColor || (Color) image[origin.X, y] == LineYMarkerColor)
            {
                // realY is accurate
            }
            else if ((Color) image[origin.X, y - 1] == LineYLabelColor || (Color) image[origin.X, y - 1] == LineYMarkerColor)
            {
                // line is one above
                realY--;
            }
            else if ((Color) image[origin.X, y + 1] == LineYLabelColor || (Color) image[origin.X, y + 1] == LineYMarkerColor)
            {
                // line is one below
                realY++;
            }
            else
            {
                throw new Exception($"Couldn't find corresponding graph line for digits at height {y}");
            }

            var number = (int) Math.Pow(10, count);
            Console.WriteLine($"Found number {number} at y-position {y} with real position of {realY}");
            numbers[y] = number;
        }

        return new YAxis(numbers);
    }

    /**
     * Gets the value for a given y coordinate (left = 0). 
     */
    public double GetValue(double y)
    {
        throw new NotImplementedException();
    }
}
