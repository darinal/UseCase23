using System.Globalization;
using CsvHelper.Configuration;
using MovieFileGenerator.Models;

namespace MovieFileGenerator.Services.Csv;

public sealed class TitleMap : ClassMap<Title>
{
    public TitleMap()
    {
        Map(t => t.Id).Name(ConvertToUnderscoreCase(nameof(Title.Id)));
        Map(t => t.TitleName).Name(ConvertToUnderscoreCase(nameof(Title.TitleName)));
        Map(t => t.Description).Name(ConvertToUnderscoreCase(nameof(Title.Description)));
        Map(t => t.ReleaseYear).Name(ConvertToUnderscoreCase(nameof(Title.ReleaseYear)));
        Map(t => t.AgeCertification).Name(ConvertToUnderscoreCase(nameof(Title.AgeCertification)));
        Map(t => t.Runtime).Name(ConvertToUnderscoreCase(nameof(Title.Runtime)));
        Map(m => m.Genres).TypeConverter<GenreListConverter>().Name(ConvertToUnderscoreCase(nameof(Title.Genres)));
        Map(t => t.ProductionCountry).Name(ConvertToUnderscoreCase(nameof(Title.ProductionCountry)));
        Map(t => t.Seasons).Name(ConvertToUnderscoreCase(nameof(Title.Seasons)));
    }

    private string ConvertToUnderscoreCase(string input)
    {
        return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
    }
}