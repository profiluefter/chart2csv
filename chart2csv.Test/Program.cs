using chart2csv.Executor;
using chart2csv.Parser.States;
using SixLabors.ImageSharp;

const string chartPath = "charts/00.0-08.0-46.0-35.0-100.0-40.0-01.0-04.0-02.0-NONE.png";

var parser = new SequentialParserExecutor(chartPath);
File.WriteAllLines("output.csv", parser.ComputeState<CSVState>().CSVLines);
parser.ComputeState<LineOverlayChartState>().LineOverlayChart.SaveAsPng("output.png");
