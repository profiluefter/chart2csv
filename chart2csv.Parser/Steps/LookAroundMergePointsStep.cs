using chart2csv.Parser.States;

namespace chart2csv.Parser.Steps;

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

            var groupX = group.First().X;

            var groupAverageY = group.Average(x => x.Y);
            var maxY = group.Max(x => x.Y);
            var minY = group.Min(x => x.Y);

            var (previousX, previousY) = processed.Last();
            var nextX = grouped[i + 1].First().X;
            var nextY = grouped[i + 1].Average(x => x.Y);

            var firstY = previousY > groupAverageY ? maxY : minY;
            var secondY = groupAverageY > nextY ? minY : maxY;

            var backXMiddle = (groupX - previousX) / 2;
            var frontXMiddle = (nextX - groupX) / 2;

            processed.Add(new Point(group.Key - backXMiddle, firstY));

            const double tolerance = 0.0001;
            if (Math.Abs(firstY - secondY) < tolerance)
            {
                processed.Add(new Point(group.Key, Math.Abs(firstY - maxY) < tolerance ? minY : maxY));
            }

            processed.Add(new Point(group.Key + frontXMiddle, secondY));
        }

        processed.Add(new Point(grouped.Last().Average(x => x.X), grouped.Last().Average(x => x.Y)));

        return new MergedChartState(input, processed);
    }
}
