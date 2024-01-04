using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public sealed record MultiFightMonsterQuestObjective
    : QuestObjective, IQuestObjective
{
    public int MonsterId { get; init; }
    public int Quantity { get; init; }

    private MultiFightMonsterQuestObjective(QuestObjectiveData questObjectiveData, int monsterId, int quantity)
        : base(questObjectiveData)
    {
        MonsterId = monsterId;
        Quantity = quantity;
    }

    internal static MultiFightMonsterQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        var parameters = questObjectiveData.Parameters;
        if (parameters.Count > 1 && int.TryParse(parameters[0], out var monsterId) && int.TryParse(parameters[1], out var quantity))
        {
            return new(questObjectiveData, monsterId, quantity);
        }

        return null;
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersData.GetMonsterDataById(MonsterId);
    }

    public Description GetDescription()
    {
        var monsterName = DofusApi.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

        return GetDescription(monsterName, Quantity);
    }
}
