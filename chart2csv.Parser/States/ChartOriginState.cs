using System.Xml.Serialization;

namespace chart2csv.Parser.States;

public class ChartOriginState : ParserState
{
    public ChartOriginState(InitialState initialState, Pixel originPoint)
    {
        InitialState = initialState;
        OriginPoint = originPoint;
    }

    public InitialState InitialState { get; }
    public Pixel OriginPoint { get; }
}
