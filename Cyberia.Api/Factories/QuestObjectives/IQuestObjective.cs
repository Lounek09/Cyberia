using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public interface IQuestObjective
    {
        QuestObjectiveData QuestObjectiveData { get; init; }

        Description GetDescription();
    }

    public interface IQuestObjective<T> : IQuestObjective where T : IQuestObjective<T>
    {
        static abstract T? Create(QuestObjectiveData questObjectiveData);
    }
}
