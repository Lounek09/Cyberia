using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record GoToNpcQuestObjective : QuestObjective, IQuestObjective<GoToNpcQuestObjective>
    {
        public int NpcId { get; init; }

        private GoToNpcQuestObjective(QuestObjectiveData questObjectiveData, int npcId) :
            base(questObjectiveData)
        {
            NpcId = npcId;
        }

        public static GoToNpcQuestObjective? Create(QuestObjectiveData questObjectiveData)
        {
            List<string> parameters = questObjectiveData.Parameters;
            if (parameters.Count > 0 && int.TryParse(parameters[0], out int npcId))
            {
                return new(questObjectiveData, npcId);
            }

            return null;
        }

        public NpcData? GetNpcData()
        {
            return DofusApi.Instance.Datacenter.NpcsData.GetNpcDataById(NpcId);
        }

        public Description GetDescription()
        {
            string npcName = DofusApi.Instance.Datacenter.NpcsData.GetNpcNameById(NpcId);

            return GetDescription(npcName);
        }
    }
}
