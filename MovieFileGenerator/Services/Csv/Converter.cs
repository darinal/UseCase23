using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace MovieFileGenerator.Services.Csv;

public class GenreListConverter : DefaultTypeConverter
{
    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value is List<string> list)
        {
            return string.Join(";", list);
        }
        return base.ConvertToString(value, row, memberMapData)!;
    }
}