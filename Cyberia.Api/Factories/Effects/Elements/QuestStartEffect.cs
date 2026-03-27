using Cyberia.Api.Data.Quests;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record QuestStartEffect : Effect, IQuestEffect
{
    public int QuestId { get; init; }

    private QuestStartEffect(int id, int questId)
        : base(id)
    {
        QuestId = questId;
    }

    internal static QuestStartEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
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
