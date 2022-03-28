namespace chart2csv.Parser.States;

public class MergedChartState : ParserState
{
    public MergedChartState(ChartWithPointsState chartWithPointsState, List<Point> points)
    {
        ChartWithPointsState = chartWithPointsState;
        Points = points;
    }

    public ChartWithPointsState ChartWithPointsState { get; }
    public List<Point> Points { get; }
}
