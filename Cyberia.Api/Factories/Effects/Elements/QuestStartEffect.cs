using Cyberia.Api.Data.Quests;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record QuestStartEffect
    : Effect, IEffect
{
    public int QuestId { get; init; }

    private QuestStartEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int questId)
        : base(id, duration, probability, criteria, effectArea)
    {
        QuestId = questId;
    }

    internal static QuestStartEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public QuestData? GetQuestData()
    {
        return DofusApi.Datacenter.QuestsData.GetQuestDataById(QuestId);
    }

    public Description GetDescription()
    {
        var questName = DofusApi.Datacenter.QuestsData.GetQuestNameById(QuestId);

        return GetDescription(string.Empty, string.Empty, questName);
    }
}
