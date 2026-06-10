namespace CsvGenerator.Generators;

public interface ICsvGenerator
{
    string Name { get; }
    string FilePath { get; }

    void Generate();
}
