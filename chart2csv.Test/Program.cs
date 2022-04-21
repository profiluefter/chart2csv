using chart2csv.Executor;
using chart2csv.Parser.States;
using SixLabors.ImageSharp;

const string chartPath = "charts/windows-chart.png";

var parser = new SequentialParserExecutor(chartPath);
parser.ComputeState<PointClusterImageState>().PointClusterOverlayChart.SaveAsPng("output-cluster.png");
parser.ComputeState<LineOverlayChartState>().LineOverlayChart.SaveAsPng("output.png");
File.WriteAllLines("output.csv", parser.ComputeState<CSVState>().CSVLines);
