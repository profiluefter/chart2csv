using Cocona;
CoconaApp.Run(([Option('i')]string input, [Option('o')]string output) => {
    //string? test = null;
    //string test2 = test ?? "";
    chart2csv.Program.RunProgram(input,output);
});
