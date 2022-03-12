namespace chart2csv.Parser.States;

public class ChartDimensionsState : ChartOriginState
{
    public ChartDimensionsState(ChartOriginState previousState, int chartWidth, int chartHeight) : base(previousState)
    {
        ChartWidth = chartWidth;
        ChartHeight = chartHeight;
    }

    protected ChartDimensionsState(ChartDimensionsState state) : this(state, state.ChartWidth, state.ChartHeight)
    {
    }

    public int ChartWidth { get; }
    public int ChartHeight { get; }
}
