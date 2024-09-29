using Cyberia.Api.Data;

using System.Diagnostics;
using System.Text;

namespace CsvGenerator.Generators;

public abstract class CsvGenerator<T>
    where T : IDofusData
{
    protected const char c_csvSeparator = ',';

    public abstract string Name { get; }
    public string FilePath => Path.Join(Program.OutputPath, $"{Name}.csv");

    protected readonly IEnumerable<T> _items;
    protected StringBuilder _builder;

    protected CsvGenerator(IEnumerable<string> columns, IEnumerable<T> items)
    {
        _items = items;
        _builder = new();

        _builder.AppendLine(string.Join(c_csvSeparator, columns));
    }

    public void Generate()
    {
        var startTime = Stopwatch.GetTimestamp();
        Log.Information($"Generating {Name} csv...");

        foreach (var item in _items)
        {
            InternalGenerate(item);
        }

        Save();

        var elapsedTime = Stopwatch.GetElapsedTime(startTime);
        Log.Information($"Generated {Name} csv in {elapsedTime.Milliseconds}ms");
    }

    protected abstract void InternalGenerate(T item);

    private void Save()
    {
        try
        {
            File.WriteAllText(FilePath, _builder.ToString());
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occured while saving the csv file.");
        }
    }
}
