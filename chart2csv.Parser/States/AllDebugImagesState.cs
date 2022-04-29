namespace chart2csv.Parser.States;

public class AllDebugImagesState : ParserState
{
    // ReSharper disable once InconsistentNaming
    public LineOverlayChartState LineOverlayChartState { get; }
    public ChartLayoutImageState ChartLayoutImageState { get; }

    public AllDebugImagesState(LineOverlayChartState overlayChartState, ChartLayoutImageState chartLayoutImageState)
    {
        LineOverlayChartState = overlayChartState;
        ChartLayoutImageState = chartLayoutImageState;
    }
}
