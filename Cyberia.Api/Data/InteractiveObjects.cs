using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class InteractiveObjectGfxData
    {
        [JsonPropertyName("id")]
        public int GfxId { get; init; }

        [JsonPropertyName("v")]
        public int InteractiveObjectId { get; init; }

        [JsonConstructor]
        internal InteractiveObjectGfxData()
        {

        }
    }

    public sealed class InteractiveObjectData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("t")]
        public int Type { get; init; }

        [JsonPropertyName("sk")]
        public List<int> SkillsId { get; init; }

        [JsonConstructor]
        internal InteractiveObjectData()
        {
            Name = string.Empty;
            SkillsId = [];
        }

        public IEnumerable<SkillData> GetSkillsData()
        {
            foreach (int skillId in SkillsId)
            {
                SkillData? skillData = DofusApi.Datacenter.SkillsData.GetSkillDataById(skillId);
                if (skillData is not null)
                {
                    yield return skillData;
                }
            }
        }
    }

    public sealed class InteractiveObjectsData
    {
        private const string FILE_NAME = "interactiveobjects.json";

        [JsonPropertyName("IO.g")]
        public List<InteractiveObjectGfxData> InteractiveObjectsGfx { get; init; }

        [JsonPropertyName("IO.d")]
        public List<InteractiveObjectData> InteractiveObjects { get; init; }

        [JsonConstructor]
        internal InteractiveObjectsData()
        {
            InteractiveObjectsGfx = [];
            InteractiveObjects = [];
        }

        internal static InteractiveObjectsData Load()
        {
            return Datacenter.LoadDataFromFile<InteractiveObjectsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public InteractiveObjectData? GetInteractiveObjectDataById(int id)
        {
            return InteractiveObjects.Find(x => x.Id == id);
        }
    }
}
