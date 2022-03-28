using chart2csv.Parser.States;
using chart2csv.Parser.Steps;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv.Executor;

public static class SequentialParserExecutor
{
    public static CSVState ParseImageToCSV(string path) => ParseImageToCSV(new InitialState(path));
    
    public static CSVState ParseImageToCSV(Image<Rgba32> image) => ParseImageToCSV(new InitialState(image));

    public static CSVState ParseImageToCSV(InitialState initialState)
    {
        var chartWithPoints = new GetPointsStep().Process(initialState);
        var chartOrigin = new FindOriginStep().Process(initialState);
        var mergedChart = new AverageMergePointsStep().Process(chartWithPoints);
        var xAxis = new DetectXAxisStep().Process(chartWithPoints);
        var yAxis = new DetectYAxisStep().Process(chartOrigin);
        var parsedChart = new ParsedChartState(mergedChart, xAxis, yAxis);
        return new GenerateCSVStep().Process(parsedChart);
    }
}
