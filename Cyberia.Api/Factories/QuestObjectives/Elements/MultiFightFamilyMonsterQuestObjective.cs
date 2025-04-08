using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record MultiFightFamilyMonsterQuestObjective : QuestObjective
{
    public int MonsterRaceId { get; init; }
    public int Quantity { get; init; }

    private MultiFightFamilyMonsterQuestObjective(QuestObjectiveData questObjectiveData, int monsterRaceId, int quantity)
        : base(questObjectiveData)
    {
        MonsterRaceId = monsterRaceId;
        Quantity = quantity;
    }

    internal static MultiFightFamilyMonsterQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        var parameters = questObjectiveData.Parameters;
        if (parameters.Count > 1 && int.TryParse(parameters[0], out var monsterRaceId) && int.TryParse(parameters[1], out var quantity))
        {
            return new(questObjectiveData, monsterRaceId, quantity);
        }

        return null;
    }

    public MonsterRaceData? GetMonsterRaceData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterRaceDataById(MonsterRaceId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var monsterRaceName = DofusApi.Datacenter.MonstersRepository.GetMonsterRaceNameById(MonsterRaceId, culture);

        return GetDescription(culture, monsterRaceName, Quantity);
    }
}
