using CsvGenerator.Extensions;

using Cyberia.Api.Data.Items;

namespace CsvGenerator.Generators.Dofusbook;

public sealed class Dofusbook_ItemTypesCsvGenerator : DofusCsvGenerator<ItemTypeData>
{
    private static readonly IReadOnlyList<string> s_columns =
    [
        "name",
        "official",
        "name_fr",
        "name_en",
        "name_es"
    ];

    public override string Name => "dofusbook_itemtypes";

    public Dofusbook_ItemTypesCsvGenerator(IEnumerable<ItemTypeData> items)
        : base(s_columns, items) { }

    protected override void AppendItem(ItemTypeData itemTypeData)
    {
        // name
        _builder.AppendCsvString(itemTypeData.Name);
        _builder.Append(c_csvSeparator);

        // official
        _builder.Append(itemTypeData.Id);
        _builder.Append(c_csvSeparator);

        // name_fr
        _builder.AppendCsvString(itemTypeData.Name.ToString(Language.fr));
        _builder.Append(c_csvSeparator);

        // name_en
        _builder.AppendCsvString(itemTypeData.Name.ToString(Language.en));
        _builder.Append(c_csvSeparator);

        // name_es
        _builder.AppendCsvString(itemTypeData.Name.ToString(Language.es));
        _builder.AppendLine();
    }
}
