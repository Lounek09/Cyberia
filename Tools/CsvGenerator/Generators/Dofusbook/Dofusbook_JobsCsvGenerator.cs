using CsvGenerator.Extensions;

using Cyberia.Api.Data.Jobs;
using Cyberia.Langzilla.Primitives;

namespace CsvGenerator.Generators.Dofusbook;

public sealed class Dofusbook_JobsCsvGenerator : DofusCsvGenerator<JobData>
{
    private static readonly IReadOnlyList<string> s_columns =
    [
        "name",
        "official",
        "name_fr",
        "name_en",
        "name_es",
        "crafts"
    ];

    public override string Name => "dofusbook_jobs";

    public Dofusbook_JobsCsvGenerator(IEnumerable<JobData> items)
        : base(s_columns, items) { }

    protected override void AppendItem(JobData item)
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
        _builder.Append(c_csvSeparator);

        //crafts
        var skillsData = item.GetSkillsData();
        if (skillsData.Any())
        {
            _builder.AppendJoin('|', skillsData.SelectMany(x => x.CraftsId));
        }
        _builder.AppendLine();
    }
}
