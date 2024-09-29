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

        if (Directory.GetDirectories(DofusApi.OutputPath).Length <= 1) //if only the custom directory is present
        {
            Log.Error("Put the generated JSON files from Salamandra in the {Path} before running this program.", DofusApi.OutputPath);
            Console.ReadKey();
            return;
        }

        CultureInfo culture = new("fr");
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        Directory.CreateDirectory(OutputPath);

        DofusApi.Initialize(new ApiConfig()
        {
            BaseLanguage = LangLanguage.fr,
            SupportedLanguages = [LangLanguage.en, LangLanguage.fr, LangLanguage.es],
            Type = LangType.Official
        });

    Retry:
        Log.Information("Wich generator do you want to run ?");
        Log.Information("1. Dofusbook_Items");
        Log.Information("2. Dofusbook_ItemSets");

        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                Dofusbook_ItemsCsvGenerator dofusbook_itemsCsvGenerator = new(DofusApi.Datacenter.ItemsRepository.Items.Values);
                dofusbook_itemsCsvGenerator.Generate();
                break;
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                Dofusbook_ItemSetsCsvGenerator dofusbook_itemSetsGenerator = new(DofusApi.Datacenter.ItemSetsRepository.ItemSets.Values);
                dofusbook_itemSetsGenerator.Generate();
                break;
            default:
                Log.Warning("Invalid choice, please try again.");
                break;
        }

        goto Retry;
    }
}
