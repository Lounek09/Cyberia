using CsvGenerator.Extensions;

using Cyberia.Api.Data.Titles;
using Cyberia.Langzilla.Enums;

namespace CsvGenerator.Generators.Dofusbook;

public sealed class Dofusbook_TitlesCsvGenerator : DofusCsvGenerator<TitleData>
{
    private static readonly IReadOnlyList<string> s_columns =
    [
        "name",
        "official",
        "name_fr",
        "name_en",
        "name_es"
    ];

    public override string Name => "dofusbook_titles";

    public Dofusbook_TitlesCsvGenerator(IEnumerable<TitleData> items)
        : base(s_columns, items)
    {

    }

    protected override void AppendItem(TitleData item)
    {
        // name
        _builder.AppendCsvString(item.Name);
        _builder.Append(c_csvSeparator);

        // official
        _builder.Append(item.Id);
        _builder.Append(c_csvSeparator);

        // name_fr
        _builder.AppendCsvString(item.Name.ToString(Language.fr));
        _builder.Append(c_csvSeparator);

        // name_en
        _builder.AppendCsvString(item.Name.ToString(Language.en));
        _builder.Append(c_csvSeparator);

        // name_es
        _builder.AppendCsvString(item.Name.ToString(Language.es));
        _builder.AppendLine();
    }
}
