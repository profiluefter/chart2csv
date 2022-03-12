namespace chart2csv.Parser.States;

public class CSVState : ParserState
{
    public CSVState(ParsedChartState previousState, List<string> csvLines)
    {
        ParsedChart = previousState;
        CSVLines = csvLines;
    }

    public ParsedChartState ParsedChart { get; }
    public List<string> CSVLines { get; }
}
