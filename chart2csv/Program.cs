using System;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace chart2csv;

internal static class Program
{
    private static readonly Color GraphHex = Color.ParseHex("3366CCFF");

    private static void Main()
    {
        var image = Image.Load<Rgba32>("charts/chart.png");

        var points = Chart2Csv.GetPoints(image, GraphHex)
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

        image.Mutate(context => context.DrawLines(
            Color.ParseHex("FF0000FF"),
            1f,
            points
                .Select(x => new PointF((float) x.X, (float) x.Y))
                .ToArray()
        ));

        var cornerColor = Color.ParseHex("B2B2B2");
        var lineColor = Color.ParseHex("C0C0C0");
        var backgroundColor = Color.ParseHex("FFFFFF");

        for (var i = 0; i < image.Width; i++)
        for (var j = 0; j < image.Height; j++)
        {
            if ((Color) image[i, j] != cornerColor ||
                (Color) image[i, j - 1] != lineColor ||
                (Color) image[i, j + 1] != backgroundColor ||
                (Color) image[i + 1, j] != lineColor ||
                (Color) image[i - 1, j] != backgroundColor) continue;
            Console.Out.WriteLine($"Chart Origin Point: {i} {j}");
            image[i, j] = Color.Aqua;
        }

        image.Save("output.png");

        //Console.WriteLine(new Point(139, 591).TranslateYToValue(1200, 0, 10000));
    }
}
