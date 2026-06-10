using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Data.Npcs;
using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record BringSoulToNpcQuestObjective : QuestObjective
{
    public int NpcId { get; init; }
    public int MonsterId { get; init; }
    public int Quantity { get; init; }

    private BringSoulToNpcQuestObjective(QuestObjectiveData questObjectiveData, int npcId, int monsterId, int quantity)
        : base(questObjectiveData)
    {
        NpcId = npcId;
        MonsterId = monsterId;
        Quantity = quantity;
    }

    internal static BringSoulToNpcQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        var parameters = questObjectiveData.Parameters;
        if (parameters.Count > 2 && int.TryParse(parameters[0], out var npcId) && int.TryParse(parameters[1], out var monsterId) && int.TryParse(parameters[2], out var quantity))
        {
            return new(questObjectiveData, npcId, monsterId, quantity);
        }

        return null;
    }

    public NpcData? GetNpcData()
    {
        return DofusApi.Datacenter.NpcsRepository.GetNpcDataById(NpcId);
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(MonsterId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var npcName = DofusApi.Datacenter.NpcsRepository.GetNpcNameById(NpcId, culture);
        var monsterName = DofusApi.Datacenter.MonstersRepository.GetMonsterNameById(MonsterId, culture);

        return GetDescription(culture, npcName, monsterName, Quantity);
    }
}

