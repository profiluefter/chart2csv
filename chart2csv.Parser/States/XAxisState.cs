namespace chart2csv.Parser.States;

public class XAxisState : ParserState
{
    private readonly int _startX;
    private readonly int _endX;
    private readonly Func<int,int,double,DateTime> _xAxisReader;

    public XAxisState(ChartWithPointsState previousState, int start, int end, Func<int, int, double, DateTime> axisReader)
    {
        ChartWithPointsState = previousState;
        _startX = start;
        _endX = end;
        _xAxisReader = axisReader;
    }
    
    public ChartWithPointsState ChartWithPointsState { get; }
    public DateTime GetXAxisValue(double x) => _xAxisReader(_startX, _endX, x);
}
