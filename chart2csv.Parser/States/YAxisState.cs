namespace chart2csv.Parser.States;

public class YAxisState : ChartOriginState
{
    private readonly Dictionary<int,int> _yAxisNumbers;
    private readonly Func<Dictionary<int,int>,double,double> _yAxisReader;

    public YAxisState(ChartOriginState previousState,
        Dictionary<int, int> yAxisNumbers,
        Func<Dictionary<int, int>, double, double> yAxisReader) : base(previousState)
    {
        _yAxisNumbers = yAxisNumbers;
        _yAxisReader = yAxisReader;
    }

    public double GetYAxisValue(double y) => _yAxisReader(_yAxisNumbers, y);
}
