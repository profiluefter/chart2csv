using chart2csv.Parser.States;

namespace chart2csv.Parser.Steps;

public class GenerateCSVStep : ParserStep<ParsedChartState, CSVState>
{
    public override CSVState Process(ParsedChartState input)
    {
        var csvLines = input.MergedChart.Points
            .Select(x => (input.XAxis.GetXAxisValue(x.X), input.YAxis.GetYAxisValue(x.Y)))
            .Select(x => FormattableString.Invariant($"{x.Item1:dd.MM.yyyy hh:mm};{x.Item2:0.######}"))
            .Prepend("DATE;BALANCE USD")
            .ToList();

        return new CSVState(input, csvLines);
    }
}
