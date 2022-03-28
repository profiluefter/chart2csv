namespace chart2csv.Parser.States;

public class ChartWithPointsState : ParserState
{
    public ChartWithPointsState(
        InitialState initialState,
        HashSet<Pixel> matchingPixels,
        HashSet<HashSet<Pixel>> rawPixelGroups,
        HashSet<Point> averagedPixelGroups)
    {
        InitialState = initialState;
        MatchingPixels = matchingPixels;
        RawPixelGroups = rawPixelGroups;
        AveragedPixelGroups = averagedPixelGroups;
    }

    public InitialState InitialState { get; }
    public HashSet<Pixel> MatchingPixels { get; }
    public HashSet<HashSet<Pixel>> RawPixelGroups { get; }
    public HashSet<Point> AveragedPixelGroups { get; }
}
