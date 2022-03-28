namespace chart2csv.Parser.States;

public class CSVState : ParserState
{
    public CSVState(ParsedChartState parsedChartState, List<string> csvLines)
    {
        ParsedChartState = parsedChartState;
        CSVLines = csvLines;
    }

    public ParsedChartState ParsedChartState { get; }
    public List<string> CSVLines { get; }
}
