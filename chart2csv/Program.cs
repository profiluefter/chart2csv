using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv
{
    internal static class Program
    {
        private static void Main()
        {
            var image = Image.Load<Rgba32>("chart.png");

            for (var j = 0; j < image.Height; j++)
            {
                var characters = "";
                for (var i = 0; i < image.Width; i++)
                {
                    if (image[i, j].ToHex() == "7D7D7DFF" && image[i - 1, j].ToHex() == "B2B2B2FF")
                    {
                        image[i, j] = Rgba32.ParseHex("FF0000FF");
                        characters += "0";
                    }
                    else if (image[i, j].ToHex() == "A0A0A0FF" && image[i - 1, j].ToHex() == "DDDDDDFF")
                    {
                        image[i, j] = Rgba32.ParseHex("00FF00FF");
                        characters += "1";
                    }
                }

                if (characters.Length == 0) continue;
                var number = int.Parse(characters);
                Console.WriteLine(number);
            }

            image.Save("output.png");
        }
    }
}
