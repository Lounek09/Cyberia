using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class SkillData
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

        public SkillData()
        {
            Description = string.Empty;
            Criterion = string.Empty;
            CraftsId = new();
        }

        public JobData? GetJobData()
        {
            return DofusApi.Datacenter.JobsData.GetJobDataById(JobId);
        }

        public InteractiveObjectData? GetInteractiveObjectData()
        {
            return DofusApi.Datacenter.InteractiveObjectsData.GetInteractiveObjectDataById(InteractiveObjectId);
        }

        public ItemTypeData? GetItemTypeDataForgemagus()
        {
            return DofusApi.Datacenter.ItemsData.GetItemTypeDataById(ItemTypeIdForgemagus);
        }

        public IEnumerable<CraftData> GetCraftsData()
        {
            foreach (int craftId in CraftsId)
            {
                CraftData? craftData = DofusApi.Datacenter.CraftsData.GetCraftDataById(craftId);
                if (craftData is not null)
                {
                    yield return craftData;
                }
            }
        }

        public ItemData? GetHarvestedItemData()
        {
            return DofusApi.Datacenter.ItemsData.GetItemDataById(HarvestedItemId);
        }
    }

    public sealed class SkillsData
    {
        private const string FILE_NAME = "skills.json";

        [JsonPropertyName("SK")]
        public List<SkillData> Skills { get; init; }

        public SkillsData()
        {
            Skills = new();
        }

        internal static SkillsData Build()
        {
            return Json.LoadFromFile<SkillsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public SkillData? GetSkillDataById(int id)
        {
            return Skills.Find(x => x.Id == id);
        }
    }
}
