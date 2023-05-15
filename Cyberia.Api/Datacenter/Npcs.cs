using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class NpcAction
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        public NpcAction()
        {
            Name = string.Empty;
        }
    }

    public sealed class Npcs
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("a")]
        public List<int> NpcActionsId { get; init; }

        public Npcs()
        {
            Name = string.Empty;
            NpcActionsId = new();
        }

        public List<NpcAction> GetNpcActions()
        {
            List<NpcAction> npcActions = new();

            foreach (int npcActionId in NpcActionsId)
            {
                NpcAction? npcAction = DofusApi.Instance.Datacenter.NpcsData.GetNpcActionById(npcActionId);
                if (npcAction is not null)
                    npcActions.Add(npcAction);
            }

            return npcActions;
        }
    }

    public sealed class NpcsData
    {
        private const string FILE_NAME = "npc.json";

        [JsonPropertyName("Na")]
        public List<NpcAction> NpcActions { get; init; }

        [JsonPropertyName("Nd")]
        public List<Npcs> Npcs { get; init; }

        public NpcsData()
        {
            NpcActions = new();
            Npcs = new();
        }

        internal static NpcsData Build()
        {
            return Json.LoadFromFile<NpcsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public NpcAction? GetNpcActionById(int id)
        {
            return NpcActions.Find(x => x.Id == id);
        }

        public Npcs? GetNpcById(int id)
        {
            return Npcs.Find(x => x.Id == id);
        }

        public string GetNpcNameById(int id)
        {
            Npcs? npc = GetNpcById(id);

            return npc is null ? $"Inconnu ({id})" : npc.Name;
        }
    }
}
