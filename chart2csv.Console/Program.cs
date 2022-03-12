using chart2csv.Executor;
using Cocona;

CoconaApp.Run((
    [Option('i', Description = "The input path for the graph")]
    string input,
    [Option('o', Description = "The output path for the graph")]
    string output,
    [Option('f', Description = "The name for the output file")]
    string? filename,
    [Option('d', Description = "Output with debug graphs")]
    bool? debug,
    [Option(Description = "Output with graph only")]
    bool? outputonly
) =>
{
    //TODO: implement debug and outputonly
    var result = SequentialParserExecutor.ParseImageToCSV(input);
    File.WriteAllLines(Path.Join(output, filename), result.CSVLines);
});
