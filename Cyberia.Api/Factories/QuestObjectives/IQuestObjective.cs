using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public interface IQuestObjective
{
    QuestObjectiveData QuestObjectiveData { get; init; }

    Description GetDescription();
}

public interface IQuestObjective<T> : IQuestObjective
{
    static abstract T? Create(QuestObjectiveData questObjectiveData);
}
