using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Bogus;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvGeneratorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a Faker generator for generating fake data
            var faker = new Faker();

            // Generate some sample data
            var records = new List<SampleData>();
            for (int i = 0; i < 10; i++)
            {
                records.Add(new SampleData
                {
                    Id = i + 1,
                    Name = faker.Name.FullName(),
                    Email = faker.Internet.Email(),
                    Age = faker.Random.Number(18, 65)
                });
            }

            // Define the path for the CSV file
            var outputPath = Path.Combine("Output", "sample_data.csv");

            // Ensure the output directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            // Write the data to the CSV file
            using (var writer = new StreamWriter(outputPath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(records);
            }

            Console.WriteLine($"CSV file generated at: {outputPath}");
        }
    }

    public class SampleData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
