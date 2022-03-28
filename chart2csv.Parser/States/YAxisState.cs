namespace chart2csv.Parser.States;

public class YAxisState : ParserState
{
    private readonly Dictionary<int,int> _yAxisNumbers;
    private readonly Func<Dictionary<int,int>,double,double> _yAxisReader;

    public YAxisState(ChartOriginState chartOriginState,
        Dictionary<int, int> yAxisNumbers,
        Func<Dictionary<int, int>, double, double> yAxisReader)
    {
        ChartOriginState = chartOriginState;
        _yAxisNumbers = yAxisNumbers;
        _yAxisReader = yAxisReader;
    }
    
    public ChartOriginState ChartOriginState { get; }
    
    public double GetYAxisValue(double y) => _yAxisReader(_yAxisNumbers, y);
}
