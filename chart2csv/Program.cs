using System;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace chart2csv;

internal static class Program
{
    private static readonly Color PointColor = Color.ParseHex("3366CC");
    private static readonly Color CornerColor = Color.ParseHex("B2B2B2");
    private static readonly Color LineColor = Color.ParseHex("C0C0C0");
    private static readonly Color LineXLabelColor = Color.ParseHex("A8A8A8");
    private static readonly Color LineYMarkerColor = Color.ParseHex("B1B1B1");
    private static readonly Color LineYLabelColor = Color.ParseHex("B2B2B2");
    private static readonly Color LineXEndColor = Color.ParseHex("D0D0D0");
    private static readonly Color LineYEndColor = Color.ParseHex("CFCFCF");
    private static readonly Color BackgroundColor = Color.ParseHex("FFFFFF");

    private static void Main()
    {
        var image = Image.Load<Rgba32>("charts/00.0-08.0-35.0-35.0-40.0-30.0-01.0-04.0-02.0-NONE.png");
        var newImage = new Image<Rgba32>(image.Width, image.Height, Color.White);

        var points = Chart2Csv.GetPoints(image, PointColor)
            .GroupBy(x => x.X)
            .OrderBy(x => x.Key)
            .Select(p => p.Count() == 1
                ? p.Single()
                : new Point(p.Average(x => x.X), p.Average(x => x.Y)))
            .ToList();

        var xAxis = XAxis.DetectXAxis(image);
        var yAxis = YAxis.DetectYAxis(image);

        var csvLines = points
            .Select(x => (xAxis.GetValue(x.X), yAxis.GetValue(x.Y)))
            .Select(x => $"{x.Item1:dd.MM.yyyy hh:mm};{x.Item2}")
            .Prepend("DATE;BALANCE USD");
            
        File.WriteAllLines("output.csv", csvLines);

        var imagePoints = points
            .Select(x => new PointF((float) x.X, (float) x.Y))
            .ToArray();

        image.Mutate(context => context.DrawLines(
            Color.Red,
            1f,
            imagePoints
        ));

        newImage.Mutate(context => context.DrawLines(
            Color.Red,
            1f,
            imagePoints
        ));

        Pixel? origin = null;

        for (var i = 0; i < image.Width; i++)
        for (var j = 0; j < image.Height; j++)
        {
            if ((Color) image[i, j] != CornerColor ||
                (Color) image[i, j - 1] != LineColor ||
                (Color) image[i, j + 1] != BackgroundColor ||
                (Color) image[i + 1, j] != LineColor ||
                (Color) image[i - 1, j] != BackgroundColor) continue;

            Console.Out.WriteLine($"Chart Origin Point: {i} {j}");
            image[i, j] = Color.Aqua;
            newImage[i, j] = Color.Aqua;

            if (origin == null)
                origin = new Pixel(i, j);
            else
                throw new Exception("Two origin points found");
        }

        if (origin == null)
            throw new Exception("No origin point found");

        Pixel originPoint = origin.Value;

        var chartWidth = 1;
        while ((Color) image[originPoint.X + chartWidth, originPoint.Y] == LineColor ||
               (Color) image[originPoint.X + chartWidth, originPoint.Y] == LineXLabelColor ||
               (Color) image[originPoint.X + chartWidth, originPoint.Y] == LineXEndColor)
        {
            image[origin.Value.X + chartWidth, origin.Value.Y] = Color.Green;
            newImage[origin.Value.X + chartWidth, origin.Value.Y] = Color.Green;
            chartWidth++;
        }

        var chartHeight = 1;
        while ((Color) image[originPoint.X, originPoint.Y - chartHeight] == LineColor ||
               (Color) image[originPoint.X, originPoint.Y - chartHeight] == LineYMarkerColor ||
               (Color) image[originPoint.X, originPoint.Y - chartHeight] == LineYLabelColor ||
               (Color) image[originPoint.X, originPoint.Y - chartHeight] == LineYEndColor)
        {
            image[originPoint.X, originPoint.Y - chartHeight] = Color.Green;
            newImage[originPoint.X, originPoint.Y - chartHeight] = Color.Green;
            chartHeight++;
        }

        chartWidth--;

        Console.WriteLine($"Chart is {chartWidth}px wide and {chartHeight}px high");

        image.Save("output.png");
        newImage.Save("output-only.png");
    }
}
