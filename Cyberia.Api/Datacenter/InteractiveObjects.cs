using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class InteractiveObjectGfxData
    {
        [JsonPropertyName("id")]
        public int GfxId { get; init; }

        [JsonPropertyName("v")]
        public int InteractiveObjectId { get; init; }

        public InteractiveObjectGfxData()
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

        public InteractiveObjectData()
        {
            Name = string.Empty;
            SkillsId = new();
        }

        public List<SkillData> GetSkillsData()
        {
            List<SkillData> skillsData = new();

            foreach (int skillId in SkillsId)
            {
                SkillData? skillData = DofusApi.Instance.Datacenter.SkillsData.GetSkillDataById(skillId);
                if (skillData is not null)
                    skillsData.Add(skillData);
            }

            return skillsData;
        }
    }

    public sealed class InteractiveObjectsData
    {
        private const string FILE_NAME = "interactiveobjects.json";

        [JsonPropertyName("IO.g")]
        public List<InteractiveObjectGfxData> InteractiveObjectsGfx { get; init; }

        [JsonPropertyName("IO.d")]
        public List<InteractiveObjectData> InteractiveObjects { get; init; }

        public InteractiveObjectsData()
        {
            InteractiveObjectsGfx = new();
            InteractiveObjects = new();
        }

        internal static InteractiveObjectsData Build()
        {
            return Json.LoadFromFile<InteractiveObjectsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public InteractiveObjectData? GetInteractiveObjectDataById(int id)
        {
            return InteractiveObjects.Find(x => x.Id == id);
        }
    }
}
