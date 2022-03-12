namespace chart2csv.Parser.States;

public class ParsedChartState : ParserState
{
    public ParsedChartState(MergedChartState mergedChart, XAxisState xAxis, YAxisState yAxis)
    {
        MergedChart = mergedChart;
        XAxis = xAxis;
        YAxis = yAxis;
    }

    public MergedChartState MergedChart { get; }
    public XAxisState XAxis { get; }
    public YAxisState YAxis { get; }
}
