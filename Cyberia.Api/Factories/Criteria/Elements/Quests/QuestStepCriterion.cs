using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Quests;

public sealed record QuestStepCriterion : Criterion
{
    public int QuestStepId { get; init; }

    private QuestStepCriterion(string id, char @operator, int questStepId)
        : base(id, @operator)
    {
        QuestStepId = questStepId;
    }

    internal static QuestStepCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var questStepId))
        {
            return new(id, @operator, questStepId);
        }

        return null;
    }

    public QuestStepData? GetQuestStepData()
    {
        return DofusApi.Datacenter.QuestsRepository.GetQuestStepDataById(QuestStepId);
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.QuestStep.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var questStepName = DofusApi.Datacenter.QuestsRepository.GetQuestStepNameById(QuestStepId, culture);

        return GetDescription(culture, questStepName);
    }
}
