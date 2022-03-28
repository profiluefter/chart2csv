namespace chart2csv.Parser.States;

public class ChartDimensionsState : ParserState
{
    public ChartDimensionsState(ChartOriginState chartOriginState, int chartWidth, int chartHeight)
    {
        ChartOriginState = chartOriginState;
        ChartWidth = chartWidth;
        ChartHeight = chartHeight;
    }

    public ChartOriginState ChartOriginState { get; }
    public int ChartWidth { get; }
    public int ChartHeight { get; }
}
