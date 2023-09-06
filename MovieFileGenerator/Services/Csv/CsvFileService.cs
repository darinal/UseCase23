using CsvHelper;
using System.Globalization;

namespace MovieFileGenerator.Services.Csv;

public class CsvFileService
{
    public void SaveToCsv<T>(IEnumerable<T> records, string filename)
    {
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.Parent!.FullName;

        string outputPath = Path.Combine(projectDirectory, "Output");

        Directory.CreateDirectory(outputPath);

        string fullFilename = Path.Combine(outputPath, filename);

        using StreamWriter writer = new StreamWriter(fullFilename);
        using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<TitleMap>();
        csv.WriteRecords(records);
    }
}