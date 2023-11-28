using Cyberia.Api.Data.Npcs;
using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public sealed record GoToNpcQuestObjective : QuestObjective, IQuestObjective<GoToNpcQuestObjective>
{
    public int NpcId { get; init; }

    private GoToNpcQuestObjective(QuestObjectiveData questObjectiveData, int npcId)
        : base(questObjectiveData)
    {
        NpcId = npcId;
    }

    public static GoToNpcQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        var parameters = questObjectiveData.Parameters;
        if (parameters.Count > 0 && int.TryParse(parameters[0], out var npcId))
        {
            return new(questObjectiveData, npcId);
        }

        return null;
    }

    public NpcData? GetNpcData()
    {
        return DofusApi.Datacenter.NpcsData.GetNpcDataById(NpcId);
    }

    public Description GetDescription()
    {
        var npcName = DofusApi.Datacenter.NpcsData.GetNpcNameById(NpcId);

        return GetDescription(npcName);
    }
}
