using chart2csv.Executor;

var result = SequentialParserExecutor.ParseImageToCSV("charts/00.0-08.0-35.0-35.0-40.0-30.0-01.0-04.0-02.0-NONE.png");
File.WriteAllLines("output.csv", result.CSVLines);
