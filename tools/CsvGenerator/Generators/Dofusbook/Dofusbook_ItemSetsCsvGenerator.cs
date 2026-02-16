using CsvGenerator.Extensions;

using Cyberia.Api.Data.ItemSets;

namespace CsvGenerator.Generators.Dofusbook;

public sealed class Dofusbook_ItemSetsCsvGenerator : DofusCsvGenerator<ItemSetData>
{
    private static readonly IReadOnlyList<string> s_columns =
    [
        "name",
        "official",
        "level",
        "no_item",
        "name_fr",
        "name_en",
        "name_es",
        "count_item",
        "effect"
    ];

    public override string Name => "dofusbook_itemsets";

    public Dofusbook_ItemSetsCsvGenerator(IEnumerable<ItemSetData> items)
        : base(s_columns, items) { }

    protected override void AppendItem(ItemSetData item)
    {
        for (var i = 0; i < item.Effects.Count; i++)
        {
            var effects = item.GetEffects(i + 1);
            if (effects.Count == 0)
            {
                continue;
            }

            // name
            _builder.AppendCsvString(item.Name);
            _builder.Append(c_csvSeparator);

            // official
            _builder.Append(item.Id);
            _builder.Append(c_csvSeparator);

            // level
            _builder.Append(item.GetLevel());
            _builder.Append(c_csvSeparator);

            // no_item
            _builder.Append(item.Effects.Count);
            _builder.Append(c_csvSeparator);

            // name_fr
            _builder.AppendCsvString(item.Name.ToString(Language.fr));
            _builder.Append(c_csvSeparator);

            // name_en
            _builder.AppendCsvString(item.Name.ToString(Language.en));
            _builder.Append(c_csvSeparator);

            // name_es
            _builder.AppendCsvString(item.Name.ToString(Language.es));
            _builder.Append(c_csvSeparator);

            // count_item
            _builder.Append(i + 1);
            _builder.Append(c_csvSeparator);

            // effect
            _builder.AppendEffects(effects);
            _builder.AppendLine();
        }
    }
}
