using System;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv
{
    internal static class Program
    {
        private const string GraphHex = "3366CCFF";

        private static void Main()
        {
            var image = Image.Load<Rgba32>("chart.png");

            Chart2Csv.GetPoints(image, GraphHex)
                .ToList()
                .ForEach(x =>
                {
                    image[(int)x.X, (int)x.Y] = Rgba32.ParseHex("FF0000FF");
                    Console.WriteLine($"{x.X}|{x.Y}");
                });
            
            image.Save("output.png");
            
            //Console.WriteLine(new Point(139, 591).TranslateYToValue(1200, 0, 10000));
        }
    }
}
