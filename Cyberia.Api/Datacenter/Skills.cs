using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Skill
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonPropertyName("j")]
        public int JobId { get; init; }

        [JsonPropertyName("io")]
        public int InteractiveObjectId { get; init; }

        [JsonPropertyName("c")]
        public string Criterion { get; init; }

        [JsonPropertyName("f")]
        public int ItemTypeIdForgemagus { get; init; }

        [JsonPropertyName("cl")]
        public List<int> CraftsId { get; init; }

        [JsonPropertyName("i")]
        public int HarvestedItemId { get; init; }

        public Skill()
        {
            Description = string.Empty;
            Criterion = string.Empty;
            CraftsId = new();
        }

        public Job? GetJob()
        {
            return DofusApi.Instance.Datacenter.JobsData.GetJobById(JobId);
        }

        public InteractiveObject? GetInteractiveObject()
        {
            return DofusApi.Instance.Datacenter.InteractiveObjectsData.GetInteractiveObjectById(InteractiveObjectId);
        }

        public ItemType? GetItemTypeForgemagus()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemTypeById(ItemTypeIdForgemagus);
        }

        public List<Craft> GetCrafts()
        {
            List<Craft> crafts = new();

            foreach (int craftId in CraftsId)
            {
                Craft? craft = DofusApi.Instance.Datacenter.CraftsData.GetCraftById(craftId);
                if (craft is not null)
                    crafts.Add(craft);
            }

            return crafts;
        }

        public Item? GetHarvestedItem()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemById(HarvestedItemId);
        }
    }

    public sealed class SkillsData
    {
        private const string FILE_NAME = "skills.json";

        [JsonPropertyName("SK")]
        public List<Skill> Skills { get; init; }

        public SkillsData()
        {
            Skills = new();
        }

        internal static SkillsData Build()
        {
            return Json.LoadFromFile<SkillsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public Skill? GetSkillById(int id)
        {
            return Skills.Find(x => x.Id == id);
        }
    }
}
