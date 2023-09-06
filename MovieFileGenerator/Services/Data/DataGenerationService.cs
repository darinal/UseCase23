using Bogus;
using MovieFileGenerator.Models;

namespace MovieFileGenerator.Services.Data;

public class DataGeneratorService
{
    private readonly List<Credit> _credits = new();
    private readonly List<Title> _titles = new();

    private readonly Faker<Title> _titleFaker;
    private readonly Faker<Credit> _creditFaker;
    private readonly Faker<Title> _nonCompliantTitleFaker;
    private readonly Faker<Credit> _nonCompliantCreditFaker;

    private int _titleId = 1;
    private int _creditId = 1;

    private readonly DataGeneratorServiceBuilder _builder;

    public DataGeneratorService(DataGeneratorServiceBuilder builder)
    {
        _titleFaker = InitializeTitleFaker();
        _creditFaker = InitializeCreditFaker();
        _nonCompliantTitleFaker = InitializeNonCompliantTitleFaker();
        _nonCompliantCreditFaker = InitializeNonCompliantCreditFaker();
        _builder = builder;
    }

    public void GenerateData()
    {
        if (_builder.TitlesWithCredits.ShouldGenerate)
        {
            GenerateCompliantMoviesWithCredits(_builder.TitlesWithCredits.Number);
        }

        if (_builder.TitlesWithoutCredits.ShouldGenerate)
        {
            GenerateCompliantMoviesWithoutCredits(_builder.TitlesWithoutCredits.Number);
        }

        if (_builder.InvalidTitlesAndCredits.ShouldGenerate)
        {
            GenerateMixedMoviesWithCredits(_builder.InvalidTitlesAndCredits.Number);
        }

        if (_builder.CreditsWithoutTitles.ShouldGenerate)
        {
            GenerateCreditsWithoutMovies(_builder.CreditsWithoutTitles.Number);
        }
    }

    public ICollection<Title> GetTitles()
    {
        return new List<Title>(_titles);
    }

    public ICollection<Credit> GetCredits()
    {
        return new List<Credit>(_credits);
    }

    private Faker<Title> InitializeTitleFaker()
    {
        return new Faker<Title>()
            .RuleFor(t => t.Id, f => _titleId++)
            .RuleFor(t => t.TitleName, GenerateTitleName)
            .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
            .RuleFor(t => t.ReleaseYear, f => f.Date.Past().Year)
            .RuleFor(t => t.AgeCertification, f => f.PickRandom(GetCertifications()))
            .RuleFor(t => t.Runtime, f => f.Random.Int(80, 180))
            .RuleFor(t => t.Genres, GetGenre)
            .RuleFor(t => t.ProductionCountry, f => f.Address.CountryCode())
            .RuleFor(t => t.Seasons, f => f.Random.Int(0, 6));
    }

    private Faker<Credit> InitializeCreditFaker()
    {
        return new Faker<Credit>()
            .RuleFor(c => c.Id, f => _creditId++)
            .RuleFor(c => c.TitleId, _titleId)
            .RuleFor(c => c.RealName, f => f.Name.FullName())
            .RuleFor(c => c.CharacterName, f => f.Name.FirstName())
            .RuleFor(c => c.Role, f => f.PickRandom(GetCreditRoles()));
    }

    private Faker<Title> InitializeNonCompliantTitleFaker()
    {
        return new Faker<Title>()
            .RuleFor(t => t.Id, f => _titleId++)
            .RuleFor(t => t.TitleName, f => f.Lorem.Word())
            .RuleFor(t => t.Genres, f => new List<string> { f.Lorem.Word() });
    }

    private Faker<Credit> InitializeNonCompliantCreditFaker()
    {
        return new Faker<Credit>()
            .RuleFor(c => c.Id, f => _creditId++)
            .RuleFor(c => c.TitleId, _titleId)
            .RuleFor(c => c.Role, f => f.Lorem.Word());
    }

    private string GenerateTitleName(Faker f) => f.PickRandom(
        $"{f.Commerce.ProductAdjective()} {f.Commerce.ProductName()}",
        $"{f.Hacker.Adjective()} {f.Hacker.Noun()}",
        $"{f.Music.Random} of {f.Lorem.Word()}",
        $"The {f.Company.Random} {f.Hacker.Verb()}",
        $"Return of the {f.Hacker.Noun()}",
        $"{f.Name.FirstName()}'s {f.Hacker.Adjective()} Adventure");

    private List<string> GetGenre(Faker f)
    {
        List<string> availableGenres = new() { "Action", "Drama", "Comedy", "Horror", "Adventure" };
        int count = f.Random.Int(1, 5);

        return f.PickRandom(availableGenres, count).ToList();
    }

    private string[] GetCertifications() => new[]
    {
        "G", "PG", "PG-13", "R", "NC-17", "U", "U/A", "A", "S", "AL", "6", "9", "12",
        "12A", "15", "18", "18R", "R18", "R21", "M", "MA15+", "R16", "R18+", "X18",
        "T", "E", "E10+", "EC", "C", "CA", "GP", "M/PG", "TV-Y", "TV-Y7", "TV-G",
        "TV-PG", "TV-14", "TV-MA"
    };

    private string[] GetCreditRoles() => new[]
    {
        "Director", "Producer", "Screenwriter", "Actor", "Actress",
        "Cinematographer", "Film Editor", "Production Designer",
        "Costume Designer", "Music Composer"
    };

    private void GenerateCompliantMoviesWithCredits(int movieCount)
    {
        for (int i = 0; i < movieCount; i++)
        {
            Title? title = _titleFaker.Generate();
            _titles.Add(title);

            int numCredits = new Random().Next(3, 10);
            for (int j = 0; j < numCredits; j++)
            {
                Credit? credit = _creditFaker.Generate();
                credit.TitleId = title.Id;
                _credits.Add(credit);
            }
        }
    }

    private void GenerateCompliantMoviesWithoutCredits(int movieCount)
    {
        _titles.AddRange(_titleFaker.Generate(movieCount));
    }

    private void GenerateCreditsWithoutMovies(int creditCount)
    {
        for (int i = 0; i < creditCount; i++)
        {
            Credit? credit = _creditFaker.Generate();
            credit.TitleId = 0;
            _credits.Add(credit);
        }
    }

    private void GenerateMixedMoviesWithCredits(int movieCount)
    {
        Random rng = new();

        for (int i = 0; i < movieCount; i++)
        {
            Title? title = rng.NextDouble() < 0.5 ? _titleFaker.Generate() : _nonCompliantTitleFaker.Generate();

            _titles.Add(title);

            _creditFaker.RuleFor(c => c.TitleId, title.Id);
            int numCredits = rng.Next(3, 10);

            for (int j = 0; j < numCredits; j++)
            {
                Credit? credit = rng.NextDouble() < 0.5 ? _creditFaker.Generate() : _nonCompliantCreditFaker.Generate();
                _credits.Add(credit);
            }
        }
    }
}