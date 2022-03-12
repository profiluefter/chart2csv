namespace chart2csv.Parser.States;

public class MergedChartState : ChartWithPointsState
{
    public MergedChartState(ChartWithPointsState previousState, List<Point> points) : base(previousState)
    {
        Points = points;
    }

    public List<Point> Points { get; }
}
