using System;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static chart2csv.Constants;

namespace chart2csv;

internal static class Program
{
    private static void Main()
    {
        var image = Image.Load<Rgba32>("charts/00.0-08.0-35.0-35.0-40.0-30.0-01.0-04.0-02.0-NONE.png");
        var newImage = new Image<Rgba32>(image.Width, image.Height);
        var pointClusterImage = new Image<Rgba32>(image.Width, image.Height);
        pointClusterImage.Mutate(context => context.DrawImage(image, 1));

        var points = Chart2Csv.GetPoints(image, PointColor, pointClusterImage)
            .GroupBy(x => x.X)
            .OrderBy(x => x.Key)
            .Select(p => p.Count() == 1
                ? p.Single()
                : new Point(p.Average(x => x.X), p.Average(x => x.Y)))
            .ToList();

        var imagePoints = points
            .Select(x => new PointF((float)x.X, (float)x.Y))
            .ToArray();

        newImage.Mutate(context => context.DrawLines(
            Color.Red,
            1f,
            imagePoints
        ));

        var origin = FindChartOrigin(image, newImage);
        var (chartWidth, chartHeight) = FindChartDimensions(image, newImage, origin);

        Console.WriteLine($"Chart is {chartWidth}px wide and {chartHeight}px high");

        var xAxis = XAxis.DetectXAxis(points[0], points[^1]);
        var yAxis = YAxis.DetectYAxis(image, newImage, origin, (chartWidth, chartHeight));

        var csvLines = points
            .Select(x => (xAxis.GetValue(x.X), yAxis.GetValue(x.Y)))
            .Select(x => $"{x.Item1:dd.MM.yyyy hh:mm};{x.Item2}")
            .Prepend("DATE;BALANCE USD");

        File.WriteAllLines("output.csv", csvLines);

        image.Mutate(context => context.DrawImage(newImage, 1));
        image.Save("output.png");
        newImage.Mutate(context => context.BackgroundColor(Color.White));
        newImage.Save("output-only.png");
        pointClusterImage.Mutate(context => context.BackgroundColor(Color.White));
        pointClusterImage.Save("point-cluster.png");
    }

    private static (int chartWidth, int chartHeight) FindChartDimensions(Image<Rgba32> image, Image<Rgba32> newImage,
        Pixel origin)
    {
        var chartWidth = 1;
        while ((Color)image[origin.X + chartWidth, origin.Y] == LineColor ||
               (Color)image[origin.X + chartWidth, origin.Y] == LineXLabelColor ||
               (Color)image[origin.X + chartWidth, origin.Y] == LineXEndColor)
        {
            newImage[origin.X + chartWidth, origin.Y] = Color.Green;
            chartWidth++;
        }

        var chartHeight = 1;
        while ((Color)image[origin.X, origin.Y - chartHeight] == LineColor ||
               (Color)image[origin.X, origin.Y - chartHeight] == LineYMarkerColor ||
               (Color)image[origin.X, origin.Y - chartHeight] == LineYLabelColor ||
               (Color)image[origin.X, origin.Y - chartHeight] == LineYEndColor)
        {
            newImage[origin.X, origin.Y - chartHeight] = Color.Green;
            chartHeight++;
        }

        // Compensate for origin point
        chartWidth--;
        chartHeight--;

        return (chartWidth, chartHeight);
    }

    private static Pixel FindChartOrigin(Image<Rgba32> image, Image<Rgba32> newImage)
    {
        Pixel? origin = null;

        for (var i = 0; i < image.Width; i++)
        for (var j = 0; j < image.Height; j++)
        {
            if ((Color)image[i, j] != LineCornerColor ||
                (Color)image[i, j - 1] != LineColor ||
                (Color)image[i, j + 1] != BackgroundColor ||
                (Color)image[i + 1, j] != LineColor ||
                (Color)image[i - 1, j] != BackgroundColor) continue;

            Console.Out.WriteLine($"Chart Origin Point: {i} {j}");
            newImage[i, j] = Color.Aqua;

            if (origin == null)
                origin = new Pixel(i, j);
            else
                throw new Exception("Two origin points found");
        }

        if (origin == null)
            throw new Exception("No origin point found");
        return origin.Value;
    }
}
