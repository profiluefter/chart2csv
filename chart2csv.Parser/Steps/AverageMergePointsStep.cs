using chart2csv.Parser.States;

namespace chart2csv.Parser.Steps;

public class AverageMergePointsStep : ParserStep<ChartWithPointsState, MergedChartState>
{
    public override MergedChartState Process(ChartWithPointsState input)
    {
        var points = input.AveragedPixelGroups
            .GroupBy(x => x.X)
            .OrderBy(x => x.Key)
            .Select(p => p.Count() == 1
                ? p.Single()
                : new Point(p.Average(x => x.X), p.Average(x => x.Y)))
            .ToList();

        return new MergedChartState(input, points);
    }
}
