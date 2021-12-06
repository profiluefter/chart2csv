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
        private const string GraphHex = "3366CCFF";

        private static void Main()
        {
            var image = Image.Load<Rgba32>("chart.png");

            var points = Chart2Csv.GetPoints(image, GraphHex)
                .GroupBy(x => x.X)
                .OrderBy(x => x.Key)
                .ToList();

            var orderedPoints = new List<Point>();

            /*
             * This loop processes each pair of points that share the same X coordinate (are above each other) and finds
             * the point that is nearest to the previous and next point. This is done to improve accuracy when
             * interpolating values to the next or previous point.
             */
            for (var index = 0; index < points.Count; index++)
            {
                var pointsHere = points[index];
                if (pointsHere.Count() == 1)
                {
                    orderedPoints.Add(pointsHere.Single());
                    continue;
                }

                var nearestPointToPrevious = pointsHere
                    .OrderBy(point => Math.Abs(point.Y - orderedPoints.Last().Y))
                    .First();
                orderedPoints.Add(nearestPointToPrevious);

                if (index + 1 == points.Count)
                    continue;
                var nextPoints = points[index + 1];
                if (nextPoints.Count() != 1)
                    continue;
                var nextPoint = nextPoints.Single();

                var nearestPointToNext = pointsHere
                    .OrderBy(point => Math.Abs(point.Y - nextPoint.Y))
                    .First();
                if (!nearestPointToNext.Equals(nearestPointToPrevious))
                    orderedPoints.Add(nearestPointToNext);
            }

            image.Mutate(context => context.DrawLines(
                Color.ParseHex("FF0000FF"),
                1f,
                orderedPoints
                    .Select(x => new PointF((float)x.X, (float)x.Y))
                    .ToArray()
            ));

            image.Save("output.png");

            //Console.WriteLine(new Point(139, 591).TranslateYToValue(1200, 0, 10000));
        }
    }
}
