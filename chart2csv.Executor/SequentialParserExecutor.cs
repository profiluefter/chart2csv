using System.Diagnostics;
using chart2csv.Parser;
using chart2csv.Parser.States;
using chart2csv.Parser.Steps;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv.Executor;

public class SequentialParserExecutor
{
    public PointMergeStrategy PointMergeStrategy { get; init; } = PointMergeStrategy.Average;
    private readonly Dictionary<Type, ParserState> _states = new();

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    public SequentialParserExecutor(InitialState initialState)
    {
        _states[typeof(InitialState)] = initialState;
    }

    public SequentialParserExecutor(Image<Rgba32> image) : this(new InitialState(image))
    {
    }

    public SequentialParserExecutor(string filePath) : this(Image.Load<Rgba32>(filePath))
    {
    }

    public T ComputeState<T>() where T : ParserState
    {
        var type = typeof(T);
        if (_states.ContainsKey(type))
            return (T)_states[type];

        var stopwatch = Stopwatch.StartNew();

        var calculatedState = (T)((ParserState)(type.Name switch
        {
            nameof(InitialState) => throw new Exception("Initial state is not defined"),

            nameof(ChartWithPointsState) => new GetPointsStep().Process(ComputeState<InitialState>()),
            nameof(ChartOriginState) => new FindOriginStep().Process(ComputeState<InitialState>()),
            nameof(MergedChartState) => PointMergeStrategy switch
            {
                PointMergeStrategy.LookAround => new LookAroundMergePointsStep().Process(
                    ComputeState<ChartWithPointsState>()),
                PointMergeStrategy.Average =>
                    new AverageMergePointsStep().Process(ComputeState<ChartWithPointsState>()),
                _ => throw new ArgumentOutOfRangeException()
            },
            nameof(XAxisState) => new DetectXAxisStep().Process(ComputeState<ChartWithPointsState>()),
            nameof(YAxisState) => new DetectYAxisStep().Process(ComputeState<ChartOriginState>()),
            nameof(ParsedChartState) => new ParsedChartState(
                ComputeState<MergedChartState>(),
                ComputeState<XAxisState>(),
                ComputeState<YAxisState>()),
            nameof(CSVState) => new GenerateCSVStep().Process(ComputeState<ParsedChartState>()),
            nameof(ChartDimensionsState) => new FindDimensionsStep().Process(ComputeState<ChartOriginState>()),
            nameof(LineOverlayChartState) => new GenerateLineOverlayStep().Process(ComputeState<MergedChartState>()),
            nameof(PointClusterImageState) => new GenerateClusterImageStep().Process(
                ComputeState<ChartWithPointsState>()),

            nameof(ChartLayoutState) => new ChartLayoutState(
                ComputeState<ChartDimensionsState>(),
                ComputeState<YAxisState>()
            ),
            nameof(ChartLayoutImageState) => new GenerateLayoutImageStep().Process(ComputeState<ChartLayoutState>()),
            nameof(AllDebugImagesState) => new AllDebugImagesState(
                ComputeState<LineOverlayChartState>(),
                ComputeState<ChartLayoutImageState>()
            ),
            nameof(CombinedDebugImageState) => new GenerateCombinedDebugImageStep().Process(
                ComputeState<AllDebugImagesState>()),

            _ => throw new Exception($"Invalid state {type.FullName}"),
        }) ?? throw new Exception("Calculated state is null"));
        stopwatch.Stop();

        Log.Debug("Calculated state {Name,-25} in {Elapsed,4} ms", type.Name, stopwatch.ElapsedMilliseconds);

        _states[type] = calculatedState;
        
        return calculatedState;
    }

    public static CSVState ParseImageToCSV(string path) =>
        new SequentialParserExecutor(path).ComputeState<CSVState>();

    public static CSVState ParseImageToCSV(Image<Rgba32> image) =>
        new SequentialParserExecutor(image).ComputeState<CSVState>();
}
