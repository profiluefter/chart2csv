using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using chart2csv.Executor;

BenchmarkRunner.Run<ChartBenchmark>();

[MemoryDiagnoser]
public class ChartBenchmark
{
    [Params(
        "00.0-08.0-35.0-35.0-40.0-30.0-01.0-04.0-02.0-NONE.png",
        "00.0-08.0-46.0-35.0-100.0-40.0-01.0-04.0-02.0-NONE.png",
        "00.0-08.0-57.0-35.0-100.0-40.0-01.0-08.0-06.0-NONE.png",
        "00.0-08.0-68.0-35.0-77.5-80.0-01.0-08.0-06.0-NONE.png",
        "00.0-08.0-79.0-80.0-100.0-80.0-01.0-12.0-06.0-NONE.png",
        "00.0-11.0-27.0-86.0-19.0-41.0-00.0-17.0-21.0-10000.0-11.0-NONE.png",
        "00.0-12.0-27.0-88.0-20.0-43.0-00.0-17.0-23.0-10000.0-13.0-NONE.png",
        "00.0-14.0-27.0-88.0-21.0-44.0-00.0-17.0-22.0-10000.0-11.0-NONE.png",
        "00.0-15.0-29.0-87.0-19.0-42.0-00.0-18.0-23.0-10000.0-11.0-NONE.png",
        "00.0-17.0-29.0-88.0-22.0-47.0-00.0-19.0-23.0-10000.0-13.0-NONE.png",
        "00.0-25.0-30.0-20000.0-24.0-27.0-00.0-02.0-10.0-NONE.png",
        "00.0-75.0-44.0-20000.0-52.0-41.0-00.0-20.0-50.0-NONE.png",
        "windows-chart.png"
    )]
    public string InputFileName { get; set; }

    [Benchmark]
    public List<string> DefaultCSV()
    {
        return new SequentialParserExecutor($"charts/{InputFileName}").ParseImageToCSV();
    }

    // [Benchmark]
    // public List<string> LookAroundMerging()
    // {
    //     return new SequentialParserExecutor($"charts/{InputFileName}")
    //     {
    //         PointMergeStrategy = PointMergeStrategy.LookAround
    //     }.ComputeState<CSVState>().CSVLines;
    // }
}
