using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record GoToNpcQuestObjective : BasicQuestObjective
    {
        public int NpcId { get; init; }

        public GoToNpcQuestObjective(QuestObjective questObjective) :
            base(questObjective)
        {
            List<string> parameters = questObjective.Parameters;

            NpcId = parameters.Count > 0 && int.TryParse(parameters[0], out int npcId) ? npcId : 0;
        }

        public static new GoToNpcQuestObjective Create(QuestObjective questObjective)
        {
            return new(questObjective);
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