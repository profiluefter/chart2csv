using System.Xml.Serialization;

namespace chart2csv.Parser.States;

public class ChartOriginState : InitialState
{
    public ChartOriginState(InitialState previousState, Pixel originPoint) : base(previousState)
    {
        OriginPoint = originPoint;
    }

    protected ChartOriginState(ChartOriginState state) : this(state, state.OriginPoint)
    {
    }

    public Pixel OriginPoint { get; }
}
