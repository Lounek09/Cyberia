using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IQuestEffect
{
    int QuestId { get; }

    QuestData? GetQuestData();
}
