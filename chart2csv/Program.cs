using System;
using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace chart2csv
{
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

            image.Mutate(context => context.DrawLines(
                Color.ParseHex("FF0000FF"),
                1f,
                points
                    .Select(x => new PointF((float)x.X, (float)x.Y))
                    .ToArray()
            ));

            image.Save("output.png");

            //Console.WriteLine(new Point(139, 591).TranslateYToValue(1200, 0, 10000));
        }
    }
}
