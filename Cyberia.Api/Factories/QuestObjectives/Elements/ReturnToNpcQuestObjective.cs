using Cyberia.Api.Data.Npcs;
using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record ReturnToNpcQuestObjective : QuestObjective
{
    public int NpcId { get; init; }

    private ReturnToNpcQuestObjective(QuestObjectiveData questObjectiveData, int npcId)
        : base(questObjectiveData)
    {
        NpcId = npcId;
    }

    internal static ReturnToNpcQuestObjective? Create(QuestObjectiveData questObjectiveData)
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

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var npcName = DofusApi.Datacenter.NpcsRepository.GetNpcNameById(NpcId, culture);

        return GetDescription(culture, npcName);
    }
}
