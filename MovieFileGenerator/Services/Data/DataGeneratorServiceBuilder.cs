namespace MovieFileGenerator.Services.Data;

public class DataGeneratorServiceBuilder
{
    public (bool ShouldGenerate, int Number) TitlesWithCredits { get; private set; }
    public (bool ShouldGenerate, int Number) TitlesWithoutCredits { get; private set; }
    public (bool ShouldGenerate, int Number) InvalidTitlesAndCredits { get; private set; }
    public (bool ShouldGenerate, int Number) CreditsWithoutTitles { get; private set; }

    public DataGeneratorServiceBuilder WithTitlesWithCredits(int numberOfTitlesWithCredits = 90)
    {
        TitlesWithCredits = (true, numberOfTitlesWithCredits);
        return this;
    }

    public DataGeneratorServiceBuilder WithTitlesWithoutCredits(int numberOfTitlesWithoutCredits = 10)
    {
        TitlesWithoutCredits = (true, numberOfTitlesWithoutCredits);
        return this;
    }

    public DataGeneratorServiceBuilder WithInvalidTitlesAndCredits(int numberOfInvalidTitlesAndCredits = 20)
    {
        InvalidTitlesAndCredits = (true, numberOfInvalidTitlesAndCredits);
        return this;
    }

    public DataGeneratorServiceBuilder WithCreditsWithoutTitles(int numberOfCreditsWithoutTitles = 10)
    {
        CreditsWithoutTitles = (true, numberOfCreditsWithoutTitles);
        return this;
    }

    public DataGeneratorService Build()
    {
        return new DataGeneratorService(this);
    }
}