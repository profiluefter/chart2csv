namespace chart2csv.Parser.States;

public class YAxisState : ParserState
{
    private readonly Func<Dictionary<int,int>,double,double> _yAxisReader;

    public Dictionary<int, int> YAxisNumbers { get; }

    public YAxisState(ChartOriginState chartOriginState,
        Dictionary<int, int> yAxisNumbers,
        Func<Dictionary<int, int>, double, double> yAxisReader)
    {
        ChartOriginState = chartOriginState;
        YAxisNumbers = yAxisNumbers;
        _yAxisReader = yAxisReader;
    }
    
    public ChartOriginState ChartOriginState { get; }
    
    public double GetYAxisValue(double y) => _yAxisReader(YAxisNumbers, y);
}
