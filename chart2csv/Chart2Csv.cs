using System;
using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv
{
    public static class Chart2Csv
    {
        public static List<Point> GetPoints(Image<Rgba32> image, string hex)
        {
            var pixels = GetPixelsWithAdjacents(image, hex);

            return GetGroupsOfPixels(pixels)
                .Select(Point.AverageFromPixels)
                .ToList();
        }

        private static List<Pixel> GetPixelsWithAdjacents(Image<Rgba32> image, string hex)
        {
            var pixels = new List<Pixel>();
            for (var x = 1; x < image.Width - 1; x++)
            {
                for (var y = 1; y < image.Height - 1; y++)
                {
                    if (image[x, y].ToHex() == hex &&
                        image[x - 1, y].ToHex() == hex &&
                        image[x + 1, y].ToHex() == hex &&
                        image[x, y - 1].ToHex() == hex &&
                        image[x, y + 1].ToHex() == hex)
                    {
                        pixels.Add(new Pixel(x, y));
                    }
                }
            }

            return pixels;
        }

        private static List<List<Pixel>> GetGroupsOfPixels(List<Pixel> pixels)
        {
            var groupsOfPixels = new List<List<Pixel>>();
            var usedPixels = new List<Pixel>();

            foreach (var groupOfPixels in pixels
                         .Where(x => !usedPixels.Contains(x))
                         .Select(x => GetGroupOfPixels(pixels, x)))
            {
                usedPixels.AddRange(groupOfPixels);
                groupsOfPixels.Add(groupOfPixels);
            }

            return groupsOfPixels;
        }

        private static List<Pixel> GetGroupOfPixels(List<Pixel> pixels, Pixel pixel, List<Pixel> exclude = null)
        {
            var group = new List<Pixel> { pixel };
            exclude ??= new List<Pixel>();
            exclude.Add(pixel);

            for (var x = -1; x <= 1; x++)
            for (var y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                var adjacent = new Pixel(pixel.X + x, pixel.Y + y);
                if (pixels.Contains(adjacent) && !exclude.Contains(adjacent))
                    group.AddRange(GetGroupOfPixels(pixels, adjacent, exclude));
            }

            return group;
        }
    }
}
