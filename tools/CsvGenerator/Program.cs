using CsvGenerator.Generators;
using CsvGenerator.Generators.Dofusbook;

using Cyberia.Api;
using Cyberia.Langzilla.Enums;

using System.Globalization;

namespace CsvGenerator;

public static class Program
{
    public const string OutputPath = "output";

    public static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u4}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        if (Directory.GetDirectories(DofusApi.OutputPath).Length <= 1) // if only the custom directory is present
        {
            Log.Error("Put the generated JSON files from Salamandra in the {Path} before running this program.", DofusApi.OutputPath);
            Console.ReadKey();
            return;
        }

        CultureInfo culture = new("fr");
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        Directory.CreateDirectory(OutputPath);

        var dofusDatacenter = DofusApi.Initialize(new DofusApiConfig()
        {
            BaseLanguage = Language.fr,
            SupportedLanguages = [Language.en, Language.fr, Language.es],
            Type = LangType.Official
        });

        Log.Information("Wich generator do you want to run ?");
        Log.Information("1. Dofusbook_Items");
        Log.Information("2. Dofusbook_ItemTypes");
        Log.Information("3. Dofusbook_ItemSets");
        Log.Information("4. Dofusbook_Spells");
        Log.Information("5. Dofusbook_Titles");

    Retry:
        ICsvGenerator? generator = Console.ReadKey(true).Key switch
        {
            ConsoleKey.D1 or ConsoleKey.NumPad1 => new Dofusbook_ItemsCsvGenerator(dofusDatacenter.ItemsRepository.Items.Values),
            ConsoleKey.D2 or ConsoleKey.NumPad2 => new Dofusbook_ItemTypesCsvGenerator(dofusDatacenter.ItemsRepository.ItemTypes.Values),
            ConsoleKey.D3 or ConsoleKey.NumPad3 => new Dofusbook_ItemSetsCsvGenerator(dofusDatacenter.ItemSetsRepository.ItemSets.Values),
            ConsoleKey.D4 or ConsoleKey.NumPad4 => new Dofusbook_SpellsCsvGenerator(dofusDatacenter.SpellsRepository.Spells.Values),
            ConsoleKey.D5 or ConsoleKey.NumPad5 => new Dofusbook_TitlesCsvGenerator(dofusDatacenter.TitlesRepository.Titles.Values),
            _ => null
        };

        if (generator is not null)
        {
            generator.Generate();
        }
        else
        {
            Log.Error("Invalid choice.");
        }

        Console.WriteLine();
        goto Retry;
    }
}
