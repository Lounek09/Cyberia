using CsvGenerator.Extensions;

using Cyberia.Api.Data.Spells;
using Cyberia.Api.Enums;
using Cyberia.Api.Factories.Effects.Elements;
using Cyberia.Langzilla.Enums;

namespace CsvGenerator.Generators.Dofusbook;

public sealed class Dofusbook_SpellsCsvGenerator : DofusCsvGenerator<SpellData>
{
    private static readonly IReadOnlyList<string> s_columns =
    [
        "name",
        "official",
        "breed",
        "summon",
        "passive",
        "name_fr",
        "name_en",
        "name_es",
        "desc_fr",
        "desc_en",
        "desc_es",
        "official_level",
        "level",
        "rank",
    ];

    public override string Name => "dofusbook_spells";

    public Dofusbook_SpellsCsvGenerator(IEnumerable<SpellData> items)
        : base(s_columns, items)
    {

    }

    protected override IEnumerable<SpellData> FilteredItems()
    {
        return _items.Where(x =>
        {
            var fromBreed = x.GetBreedData() is not null;
            var fromIncarnation = x.GetIncarnationData() is not null;
            var isCommon = x.SpellCategory is SpellCategory.Elementary or SpellCategory.Summon;

            return fromBreed || (isCommon && !fromIncarnation);
        });
    }

    protected override void AppendItem(SpellData item)
    {
        foreach (var spellLevelData in item.GetSpellLevelsData())
        {
            //name
            _builder.AppendCsvString(item.Name);
            _builder.Append(c_csvSeparator);

            //official
            _builder.Append(item.Id);
            _builder.Append(c_csvSeparator);

            //breed
            var breedData = item.GetBreedData();
            if (breedData is not null)
            {
                _builder.AppendCsvString(breedData.Name);
            }
            else if (item.SpellCategory is SpellCategory.Elementary or SpellCategory.Summon)
            {
                _builder.AppendCsvString("Commun");
            }
            _builder.Append(c_csvSeparator);

            //summon
            var summon = spellLevelData.Effects.Any(x => x is SummonCreatureEffect or SummonStaticCreatureEffect);
            if (summon)
            {
                _builder.Append('x');
            }
            _builder.Append(c_csvSeparator);

            //passive
            if (item.Passive)
            {
                _builder.Append('x');
            }
            _builder.Append(c_csvSeparator);

            //name_fr
            _builder.AppendCsvString(item.Name.ToString(Language.fr));
            _builder.Append(c_csvSeparator);

            //name_en
            _builder.AppendCsvString(item.Name.ToString(Language.en));
            _builder.Append(c_csvSeparator);

            //name_es
            _builder.AppendCsvString(item.Name.ToString(Language.es));
            _builder.Append(c_csvSeparator);

            //desc_fr
            _builder.AppendCsvString(item.Description.ToString(Language.fr));
            _builder.Append(c_csvSeparator);

            //desc_en
            _builder.AppendCsvString(item.Description.ToString(Language.en));
            _builder.Append(c_csvSeparator);

            //desc_es
            _builder.AppendCsvString(item.Description.ToString(Language.es));
            _builder.Append(c_csvSeparator);

            //official_level
            _builder.Append(spellLevelData.Id);
            _builder.Append(c_csvSeparator);

            //level
            _builder.Append(spellLevelData.NeededLevel);
            _builder.Append(c_csvSeparator);

            //rank
            _builder.Append(spellLevelData.Rank);
            _builder.AppendLine();
        }
    }
}
