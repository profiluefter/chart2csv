using System;
using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv
{
    public static class Chart2Csv
    {
        public static HashSet<Point> GetPoints(Image<Rgba32> image, Color hex) =>
            GetGroupsOfPixels(GetPixelsWithAdjacents(image, hex))
                .Select(pixels => new Point(
                    pixels.Average(x => x.X),
                    pixels.Average(x => x.Y)
                ))
                .ToHashSet();

        /**
         * Searches an image for plus-shaped patterns where all pixels have the specified color.
         */
        private static HashSet<Pixel> GetPixelsWithAdjacents(Image<Rgba32> image, Color hex)
        {
            var pixels = new HashSet<Pixel>();
            for (var x = 1; x < image.Width - 1; x++)
            for (var y = 1; y < image.Height - 1; y++)
                if ((Color)image[x, y] == hex &&
                    (Color)image[x - 1, y] == hex &&
                    (Color)image[x + 1, y] == hex &&
                    (Color)image[x, y - 1] == hex &&
                    (Color)image[x, y + 1] == hex)
                {
                    pixels.Add(new Pixel(x, y));
                }

            return pixels;
        }

        private static HashSet<HashSet<Pixel>> GetGroupsOfPixels(HashSet<Pixel> pixels)
        {
            var groupsOfPixels = new HashSet<HashSet<Pixel>>();
            var usedPixels = new HashSet<Pixel>();

            foreach (var groupOfPixels in pixels
                         .Where(x => !usedPixels.Contains(x))
                         .Select(x => GetGroupOfPixels(pixels, x)))
            {
                usedPixels.UnionWith(groupOfPixels);
                groupsOfPixels.Add(groupOfPixels);
            }

            return groupsOfPixels;
        }

        private static HashSet<Pixel> GetGroupOfPixels(HashSet<Pixel> pixels, Pixel pixel,
            HashSet<Pixel> exclude = null)
        {
            var group = new HashSet<Pixel> { pixel };
            exclude ??= new HashSet<Pixel>();
            exclude.Add(pixel);

            for (var x = -1; x <= 1; x++)
            for (var y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                var adjacent = new Pixel(pixel.X + x, pixel.Y + y);
                if (pixels.Contains(adjacent) && !exclude.Contains(adjacent))
                    group.UnionWith(GetGroupOfPixels(pixels, adjacent, exclude));
            }

            return group;
        }
    }
}
