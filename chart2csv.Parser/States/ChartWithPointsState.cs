namespace chart2csv.Parser.States;

public class ChartWithPointsState : InitialState
{
    public ChartWithPointsState(
        InitialState previousState,
        HashSet<Pixel> matchingPixels,
        HashSet<HashSet<Pixel>> rawPixelGroups,
        HashSet<Point> averagedPixelGroups) : base(previousState)
    {
        MatchingPixels = matchingPixels;
        RawPixelGroups = rawPixelGroups;
        AveragedPixelGroups = averagedPixelGroups;
    }

    protected ChartWithPointsState(ChartWithPointsState state)
        : this(state, state.MatchingPixels, state.RawPixelGroups, state.AveragedPixelGroups)
    {
    }

    public HashSet<Pixel> MatchingPixels { get; }
    public HashSet<HashSet<Pixel>> RawPixelGroups { get; }
    public HashSet<Point> AveragedPixelGroups { get; }
}
