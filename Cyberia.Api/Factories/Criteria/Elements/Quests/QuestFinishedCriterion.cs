using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Quests;

public sealed record QuestFinishedCriterion : Criterion
{
    public int QuestId { get; init; }

    private QuestFinishedCriterion(string id, char @operator, int questId)
        : base(id, @operator)
    {
        QuestId = questId;
    }

    internal static QuestFinishedCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var questId))
        {
            return new(id, @operator, questId);
        }

        return null;
    }

    public QuestData? GetQuestData()
    {
        return DofusApi.Datacenter.QuestsRepository.GetQuestDataById(QuestId);
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.QuestFinished.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var questName = DofusApi.Datacenter.QuestsRepository.GetQuestNameById(QuestId, culture);

        return GetDescription(culture, questName);
    }
}
