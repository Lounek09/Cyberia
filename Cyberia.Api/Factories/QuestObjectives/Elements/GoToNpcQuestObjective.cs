using Cyberia.Api.Data.Npcs;
using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public sealed record GoToNpcQuestObjective : QuestObjective
{
    public int NpcId { get; init; }

    private GoToNpcQuestObjective(QuestObjectiveData questObjectiveData, int npcId)
        : base(questObjectiveData)
    {
        NpcId = npcId;
    }

    internal static GoToNpcQuestObjective? Create(QuestObjectiveData questObjectiveData)
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
        return DofusApi.Datacenter.NpcsRepository.GetNpcDataById(NpcId);
    }

    public override DescriptionString GetDescription()
    {
        var npcName = DofusApi.Datacenter.NpcsRepository.GetNpcNameById(NpcId);

        return GetDescription(npcName);
    }
}
