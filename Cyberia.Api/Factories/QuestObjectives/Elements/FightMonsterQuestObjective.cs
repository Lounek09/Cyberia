using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

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

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var monsterName = DofusApi.Datacenter.MonstersRepository.GetMonsterNameById(MonsterId, culture);

        return GetDescription(culture, monsterName);
    }
}
