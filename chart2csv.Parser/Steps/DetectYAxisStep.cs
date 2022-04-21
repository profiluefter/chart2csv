using chart2csv.Parser.States;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv.Parser.Steps;

public class DetectYAxisStep : ParserStep<ChartOriginState, YAxisState>
{
    private static readonly Color NumberZeroLinuxColor = Color.ParseHex("7A7A7A");
    private static readonly Color NumberZeroWindowsColorLeft = Color.ParseHex("B2B2B2");
    private static readonly Color NumberZeroWindowsColorRight = Color.ParseHex("8E8E8E");
    
    private static readonly Color LineYMarkerColor = Color.ParseHex("B1B1B1");
    private static readonly Color LineYLabelColor = Color.ParseHex("B2B2B2");

    private static readonly double LogarithmPercentage = 1-Math.Log10(9);
    
    public override YAxisState Process(ChartOriginState input)
    {
        var origin = input.OriginPoint;
        var image = input.InitialState.InputImage;
        
        var digits = FindDigits(origin, image);

        var numbers = DigitsToNumbers(image, origin, digits);

        switch (numbers.Count) {
            case > 1: {
                var firstTwoNumbers = numbers
                    .OrderByDescending(x => x.Key)
                    .Take(2)
                    .ToArray();
            
                var diff = firstTwoNumbers[0].Key - firstTwoNumbers[1].Key;
                numbers[firstTwoNumbers[0].Key + diff] = firstTwoNumbers[0].Value / 10;
                break;
            }
            case 1: {
                var firstNumber = numbers
                    .OrderByDescending(x => x.Key)
                    .First();

                for (var i = firstNumber.Key+1; i < origin.Y; i++) {
                
                    if ((Color) image[origin.X, i] != LineYMarkerColor) continue;
                
                    var diff = i - firstNumber.Key;
                    var actualDiff = (int) (diff / LogarithmPercentage);
                    numbers[firstNumber.Key + actualDiff] = firstNumber.Value / 10;
                        
                    break;
                }

                break;
            }
        }

        return new YAxisState(input, numbers, GetValue);
    }

    private static Dictionary<int, int> FindDigits(Pixel origin, Image<Rgba32> image)
    {
        var linuxDigits = new Dictionary<int, int>();
        var windowsDigits = new Dictionary<int, int>();

        for (var x = 2; x < origin.X; x++)
        for (var y = 0; y < origin.Y; y++)
        {
            // search for linux pattern
            if ((Color)image[x - 2, y] == NumberZeroLinuxColor && (Color)image[x + 2, y] == NumberZeroLinuxColor)
            {
                if (!linuxDigits.ContainsKey(y))
                    linuxDigits[y] = 0;
                linuxDigits[y]++;
            }
            
            // search for windows pattern
            if ((Color)image[x - 2, y] == NumberZeroWindowsColorLeft &&
                (Color)image[x + 2, y] == NumberZeroWindowsColorRight)
            {
                if (!windowsDigits.ContainsKey(y))
                    windowsDigits[y] = 0;
                windowsDigits[y]++;
            }
        }

        Console.WriteLine($"Found {linuxDigits.Count} linux digits and {windowsDigits.Count} windows digits");
        var isWindows = windowsDigits.Count > linuxDigits.Count;
        Console.WriteLine($"Assuming this is a {(isWindows ? "windows" : "linux")} chart");

        return isWindows ? windowsDigits : linuxDigits;
    }

    private static Dictionary<int, int> DigitsToNumbers(Image<Rgba32> image, Pixel origin, Dictionary<int, int> digits)
    {
        var numbers = new Dictionary<int, int>();

        foreach (var (y, count) in digits)
        {
            var realY = y;
            if ((Color)image[origin.X, y] == LineYLabelColor || (Color)image[origin.X, y] == LineYMarkerColor)
            {
                // realY is accurate
            }
            else if ((Color)image[origin.X, y - 1] == LineYLabelColor || (Color)image[origin.X, y - 1] == LineYMarkerColor)
            {
                // line is one above
                realY--;
            }
            else if ((Color)image[origin.X, y + 1] == LineYLabelColor || (Color)image[origin.X, y + 1] == LineYMarkerColor)
            {
                // line is one below
                realY++;
            }
            else
            {
                throw new Exception($"Couldn't find corresponding graph line for digits at height {y}");
            }

            var number = (int)Math.Pow(10, count);
            numbers[realY] = number;
        }

        return numbers;
    }

    private static double GetValue(Dictionary<int, int> numbers, double y) {
        if (numbers.Count < 2) throw new Exception("not enough markers to calculate y value"); 
            
        var (key, value) = numbers.OrderByDescending(x => x.Key).Last(x => x.Key > y);
        var totalPixels = numbers.OrderByDescending(x => x.Key).First().Key 
                          - numbers.OrderByDescending(x => x.Key).Skip(1).First().Key;
        return Math.Pow(10, (key - y) / totalPixels) * value;
    }
}
