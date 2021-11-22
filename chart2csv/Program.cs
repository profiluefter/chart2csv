using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv
{
    class Program
    {
        static void Main(string[] args)
        {
            var image = Image.Load<Rgba32>("chart.png");
            Console.WriteLine(image[0,0]);
        }
    }
}
