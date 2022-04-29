namespace chart2csv.Parser.States;

public class ChartLayoutState : ParserState
{
    public ChartDimensionsState ChartDimensionsState { get; }
    public YAxisState YAxisState { get; }

    public ChartLayoutState(ChartDimensionsState chartDimensionsState, YAxisState yAxisState)
    {
        ChartDimensionsState = chartDimensionsState;
        YAxisState = yAxisState;
    }
}
