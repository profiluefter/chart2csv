using System.Diagnostics;
using chart2csv.Executor;
using chart2csv.Parser;
using chart2csv.Parser.States;
using Cocona;
using Serilog;
using Serilog.Events;
using SixLabors.ImageSharp;

const string banner = @"
          $$\                            $$\      $$$$$$\                                 
          $$ |                           $$ |    $$  __$$\                                
 $$$$$$$\ $$$$$$$\   $$$$$$\   $$$$$$\ $$$$$$\   \__/  $$ | $$$$$$$\  $$$$$$$\ $$\    $$\ 
$$  _____|$$  __$$\  \____$$\ $$  __$$\\_$$  _|   $$$$$$  |$$  _____|$$  _____|\$$\  $$  |
$$ /      $$ |  $$ | $$$$$$$ |$$ |  \__| $$ |    $$  ____/ $$ /      \$$$$$$\   \$$\$$  / 
$$ |      $$ |  $$ |$$  __$$ |$$ |       $$ |$$\ $$ |      $$ |       \____$$\   \$$$  /  
\$$$$$$$\ $$ |  $$ |\$$$$$$$ |$$ |       \$$$$  |$$$$$$$$\ \$$$$$$$\ $$$$$$$  |   \$  /   
 \_______|\__|  \__| \_______|\__|        \____/ \________| \_______|\_______/     \_/    
";

void Program(
    [Argument(Description = "The path or filename of the image of the graph that should be converted")]
    string input,
    [Argument(Description = "Folder or filename to output the csv file to")]
    string? output,
    [Option("merge-strategy", Description = "The method to use when merging points with the same X position")]
    PointMergeStrategy pointMergeStrategy = PointMergeStrategy.Average,
    [Option("debug-files", Description = "If specified, debug graphics will be generated into the specified folder")]
    string? debugFiles = null,
    [Option("overwrite", Description = "Overwrite the output file if it already exists")]
    bool overwrite = false,
    [Option("log-level", new[] { 'l' }, Description = "The log level of the console logger")]
    LogEventLevel logLevel = LogEventLevel.Information,
    [Option("verbose", new[] { 'v' }, Description = "Outputs more information, alias for --log-level=Verbose")]
    bool verbose = false,
    [Option("silent", new[] { 's' }, Description = "Silent mode, no output to console")]
    bool silent = false
)
{
    // -------- Setup logging --------

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Is(verbose ? LogEventLevel.Verbose : logLevel)
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        .Filter.ByExcluding(_ => silent)
        .CreateLogger();

    Log.Verbose(banner);

    // ------- Input validation -------

    if (!File.Exists(input))
    {
        Log.Fatal("File not found: {Input}", input);
        Environment.Exit(1);
    }

    Log.Debug("Input file: {Input}", input);

    output ??= Path.GetDirectoryName(input)!;

    if (Path.EndsInDirectorySeparator(output) && !Directory.Exists(output))
    {
        Log.Fatal("Output directory not found: {Output}", Path.GetFullPath(output));
        Environment.Exit(1);
    }

    // Check if the output file is either a folder or a file. If it is a folder use a default file name
    if (Directory.Exists(output))
    {
        output = Path.Combine(output, Path.GetFileNameWithoutExtension(input) + ".csv");
    }
    else if (!Path.HasExtension(output))
    {
        output += ".csv";
    }

    if (File.Exists(output))
    {
        if (overwrite)
        {
            Log.Warning("Output file already exists, but will be overwritten: {Output}", output);
        }
        else
        {
            Log.Fatal("Output file already exists: {Output}", output);
            Log.Information("Use --overwrite to overwrite the file");
            Environment.Exit(1);
        }
    }

    Log.Debug("Output file: {Output}", output);

    // if debug files are specified, create the directory and check that it doesn't already exist
    if (debugFiles != null)
    {
        if (Directory.Exists(debugFiles))
        {
            Log.Fatal("Debug files directory already exists: {DebugFiles}", debugFiles);
            Environment.Exit(1);
        }

        Directory.CreateDirectory(debugFiles);
    }

    // -------- Convert the chart --------

    var stopwatch = Stopwatch.StartNew();

    SequentialParserExecutor executor;
    CSVState csvState;
    try
    {
        executor = new SequentialParserExecutor(input);
        csvState = executor.ComputeState<CSVState>();
    }
    catch (ParserException e)
    {
        Log.Fatal("A fatal error occurred: {Message}", e.Message);
        Environment.Exit(1);
        return; // Unreachable
    }

    stopwatch.Stop();

    Log.Debug("Writing CSV file");
    File.WriteAllLines(output, csvState.CSVLines);

    Log.Information("Done. Generated {Count} CSV lines into {FileName} in {Millis} ms",
        csvState.CSVLines.Count, output, stopwatch.ElapsedMilliseconds);

    // -------- Generate debug files --------

    if (debugFiles != null)
    {
        Log.Debug("Generating debug files");

        try
        {
            executor.ComputeState<LineOverlayChartState>().LineOverlayChart
                .SaveAsPng(Path.Combine(debugFiles, "line-overlay.png"));
            executor.ComputeState<PointClusterImageState>().PointClusterOverlayChart
                .SaveAsPng(Path.Combine(debugFiles, "point-cluster.png"));
            executor.ComputeState<ChartLayoutImageState>().ChartLayoutImage
                .SaveAsPng(Path.Combine(debugFiles, "chart-layout.png"));
            executor.ComputeState<CombinedDebugImageState>().CombinedDebugImage
                .SaveAsPng(Path.Combine(debugFiles, "combined-overlay.png"));
        }
        catch (ParserException e)
        {
            Log.Fatal("Failed to generate debug files: {Message}", e.Message);
            Environment.Exit(1);
        }

        Log.Information("Debug files written to {DebugFiles}", debugFiles);
    }
}

CoconaApp.Run(Program);
