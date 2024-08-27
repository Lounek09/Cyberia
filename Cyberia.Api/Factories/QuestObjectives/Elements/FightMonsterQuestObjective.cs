using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public sealed record FightMonsterQuestObjective : QuestObjective
{
    public int MonsterId { get; init; }

    private FightMonsterQuestObjective(QuestObjectiveData questObjectiveData, int monsterId)
        : base(questObjectiveData)
    {
        MonsterId = monsterId;
    }

    internal static FightMonsterQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        var parameters = questObjectiveData.Parameters;
        if (parameters.Count > 0 && int.TryParse(parameters[0], out var monsterId))
        {
            return new(questObjectiveData, monsterId);
        }

        return null;
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(MonsterId);
    }

    public override DescriptionString GetDescription()
    {
        var monsterName = DofusApi.Datacenter.MonstersRepository.GetMonsterNameById(MonsterId);

        return GetDescription(monsterName);
    }
}
