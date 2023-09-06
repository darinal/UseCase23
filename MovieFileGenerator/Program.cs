using MovieFileGenerator.Models;
using MovieFileGenerator.Services.Csv;
using MovieFileGenerator.Services.Data;

namespace MovieFileGenerator;

public class Program
{
    public static void Main()
    {
        DataGeneratorService dataGeneratorService = new DataGeneratorServiceBuilder()
            .WithTitlesWithCredits()
            .WithTitlesWithoutCredits()
            .WithInvalidTitlesAndCredits()
            .WithCreditsWithoutTitles()
            .Build();

        dataGeneratorService.GenerateData();

        ICollection<Title> titles = dataGeneratorService.GetTitles();
        ICollection<Credit> credits = dataGeneratorService.GetCredits();

        CsvFileService fileService = new();
        fileService.SaveToCsv(titles, "titles.csv");
        fileService.SaveToCsv(credits, "credits.csv");

        Print(titles, credits);
    }

    private static void Print(ICollection<Title> titles, ICollection<Credit> credits)
    {
        foreach (Title title in titles)
        {
            Console.WriteLine($"{title.Id}, {title.TitleName} ({title.AgeCertification}) - {title.ReleaseYear}");
            Console.Write("Cast: ");

            List<Credit> relatedCredits = credits.Where(c => c.TitleId == title.Id).ToList();
            if (relatedCredits.Count <= 0)
            {
                Console.Write("none");
                Console.WriteLine();
                Console.WriteLine();

                continue;
            }

            for (int i = 0; i < relatedCredits.Count; i++)
            {
                Console.Write($"{relatedCredits[i].Role}: {relatedCredits[i].RealName}");
                if (i < relatedCredits.Count - 1)
                {
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}