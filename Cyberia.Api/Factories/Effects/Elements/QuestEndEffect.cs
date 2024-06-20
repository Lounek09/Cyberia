using Cyberia.Api.Data.Quests;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record QuestEndEffect : Effect, IEffect
{
    public int QuestId { get; init; }

    private QuestEndEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int questId)
        : base(id, duration, probability, criteria, effectArea)
    {
        QuestId = questId;
    }

    internal static QuestEndEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public QuestData? GetQuestData()
    {
        return DofusApi.Datacenter.QuestsRepository.GetQuestDataById(QuestId);
    }

    public Description GetDescription()
    {
        var questName = DofusApi.Datacenter.QuestsRepository.GetQuestNameById(QuestId);

        return GetDescription(string.Empty, string.Empty, questName);
    }
}
