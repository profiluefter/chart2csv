namespace chart2csv.Parser;

public abstract class ParserStep<TIn, TOut>
    where TIn : ParserState
    where TOut : ParserState
{
    public abstract TOut Process(TIn input);
}
