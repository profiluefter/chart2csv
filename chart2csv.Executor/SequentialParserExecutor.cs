using System.Diagnostics;
using chart2csv.Parser;
using chart2csv.Parser.States;
using chart2csv.Parser.Steps;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace chart2csv.Executor;

public class SequentialParserExecutor
{
    public bool UseLookAroundMerging { get; init; } = false;
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

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var calculatedState = (T)((ParserState)(type.Name switch
        {
            nameof(InitialState) => throw new Exception("Initial state is not defined"),

            nameof(ChartWithPointsState) => new GetPointsStep().Process(ComputeState<InitialState>()),
            nameof(ChartOriginState) => new FindOriginStep().Process(ComputeState<InitialState>()),
            nameof(MergedChartState) => UseLookAroundMerging
                ? new LookAroundMergePointsStep().Process(ComputeState<ChartWithPointsState>())
                : new AverageMergePointsStep().Process(ComputeState<ChartWithPointsState>()),
            nameof(XAxisState) => new DetectXAxisStep().Process(ComputeState<ChartWithPointsState>()),
            nameof(YAxisState) => new DetectYAxisStep().Process(ComputeState<ChartOriginState>()),
            nameof(ParsedChartState) => new ParsedChartState(
                ComputeState<MergedChartState>(),
                ComputeState<XAxisState>(),
                ComputeState<YAxisState>()),
            nameof(CSVState) => new GenerateCSVStep().Process(ComputeState<ParsedChartState>()),
            nameof(ChartDimensionsState) => new FindDimensionsStep().Process(ComputeState<ChartOriginState>()),
            nameof(LineOverlayChartState) => new GenerateLineOverlayStep().Process(ComputeState<MergedChartState>()),
            nameof(PointClusterImageState) => new GenerateClusterImage().Process(ComputeState<ChartWithPointsState>()),

            _ => throw new Exception($"Invalid state {type.FullName}"),
        }) ?? throw new Exception("Calculated state is null"));
        stopwatch.Stop();
        
        Console.WriteLine($"{type.Name,-30}: {stopwatch.ElapsedMilliseconds.ToString(),4} ms");
        
        _states[type] = calculatedState;
        
        return calculatedState;
    }

    public static CSVState ParseImageToCSV(string path) =>
        new SequentialParserExecutor(path).ComputeState<CSVState>();

    public static CSVState ParseImageToCSV(Image<Rgba32> image) =>
        new SequentialParserExecutor(image).ComputeState<CSVState>();
}
