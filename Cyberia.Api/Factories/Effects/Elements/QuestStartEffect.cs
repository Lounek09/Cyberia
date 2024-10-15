using Cyberia.Api.Data.Quests;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record QuestStartEffect : Effect
{
    public int QuestId { get; init; }

    private QuestStartEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int questId)
        : base(id, duration, probability, criteria, effectArea)
    {
        QuestId = questId;
    }

    internal static QuestStartEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public QuestData? GetQuestData()
    {
        return DofusApi.Datacenter.QuestsRepository.GetQuestDataById(QuestId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var questName = DofusApi.Datacenter.QuestsRepository.GetQuestNameById(QuestId, culture);

        return GetDescription(culture, string.Empty, string.Empty, questName);
    }
}
