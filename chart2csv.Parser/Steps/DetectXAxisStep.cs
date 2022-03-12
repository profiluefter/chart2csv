using chart2csv.Parser.States;

namespace chart2csv.Parser.Steps;

public class DetectXAxisStep : ParserStep<ChartWithPointsState, XAxisState>
{
    private static readonly DateTime StartDate = new(2018, 01, 01, 23, 00, 00);
    private static readonly DateTime EndDate = new(2021, 02, 01, 06, 00, 00);

    public override XAxisState Process(ChartWithPointsState input)
    {
        var pixelGroups = input.AveragedPixelGroups.OrderBy(x => x.X).ToList();
        
        var start = pixelGroups.First();
        var end = pixelGroups.Last();

        return new XAxisState(input, (int)start.X, (int)end.X, GetValue);
    }
    
    /**
     * Gets the date value for a given x coordinate (top = 0).
     */
    private static DateTime GetValue(int startX, int endX, double x) // TODO: check if the data not accurate or there is an error here
    {
        var startValue = new DateTimeOffset(StartDate).ToUnixTimeSeconds();
        var endValue = new DateTimeOffset(EndDate).ToUnixTimeSeconds();
        var valueRange = endValue - startValue;

        var range = endX - startX;
        var relativePosition = x - startX;

        var percent = relativePosition / range;

        var value = percent * valueRange + startValue;
        return DateTimeOffset.FromUnixTimeSeconds((long) value).DateTime;
    }
}
