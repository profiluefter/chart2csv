using chart2csv.Parser.States;

namespace chart2csv.Parser.Steps;

//TODO: Debug and add logging
public class LookAroundMergePointsStep : ParserStep<ChartWithPointsState, MergedChartState>
{
    public override MergedChartState Process(ChartWithPointsState input)
    {
        var grouped = input.AveragedPixelGroups
            .GroupBy(x => x.X)
            .ToList();

        var processed = new List<Point>(grouped.Count) {grouped[0].Single()};
        for (var i = 1; i < grouped.Count - 1; i++)
        {
            var group = grouped[i];
            if (group.Count() < 2)
            {
                processed.Add(group.Single());
                continue;
            }

            var groupAverageY = group.Average(x => x.Y);
            var maxY = group.Max(x => x.Y);
            var minY = group.Min(x => x.Y);

            var previousY = processed.Last().Y;
            var nextY = grouped[i + 1].Average(x => x.Y);

            var firstY = previousY > groupAverageY ? maxY : minY;
            var secondY = groupAverageY > nextY ? minY : maxY;

            processed.Add(new Point(group.Key - 0.01, firstY));
            processed.Add(new Point(group.Key + 0.01, secondY));

            if (Math.Abs(firstY - secondY) < 0.01)
                Console.Error.WriteLine("firstY == secondY: This is probably a bug");

            var orientation = firstY > secondY ? "declining" : "ascending";

            Console.WriteLine($"Merging at X={group.Key:0000}: previousY={previousY:000} nextY={nextY:000} " +
                              $"maxY={maxY:000} minY={minY:000} firstY={firstY:000} secondY={secondY:000} " +
                              $"orientation={orientation}");
        }

        processed.Add(new Point(grouped.Last().Average(x => x.X), grouped.Last().Average(x => x.Y)));

        return new MergedChartState(input, processed);
    }
}
