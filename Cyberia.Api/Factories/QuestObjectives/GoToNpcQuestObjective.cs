using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed class GoToNpcQuestObjective : BasicQuestObjective
    {
        public int NpcId { get; init; }

        public GoToNpcQuestObjective(QuestObjective questObjective, int npcId) :
            base(questObjective)
        {
            NpcId = npcId;
        }

        public static new GoToNpcQuestObjective? Create(QuestObjective questObjective)
        {
            if (questObjective.Parameters.Count > 0 &&
                int.TryParse(questObjective.Parameters[0], out int npcId))
                return new(questObjective, npcId);

            return null;
        }

        public Npcs? GetNpc()
        {
            return DofusApi.Instance.Datacenter.NpcsData.GetNpcById(NpcId);
        }

        public override string GetDescription()
        {
            string npcName = DofusApi.Instance.Datacenter.NpcsData.GetNpcNameById(NpcId);

            return GetDescriptionFromParameters(npcName);
        }
    }
}