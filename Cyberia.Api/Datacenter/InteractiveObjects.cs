using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class InteractiveObjectGfx
    {
        [JsonPropertyName("id")]
        public int GfxId { get; init; }

        [JsonPropertyName("v")]
        public int InteractiveObjectId { get; init; }

        public InteractiveObjectGfx()
        {

        }
    }

    public sealed class InteractiveObject
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("t")]
        public int Type { get; init; }

        [JsonPropertyName("sk")]
        public List<int> SkillsId { get; init; }

        public InteractiveObject()
        {
            Name = string.Empty;
            SkillsId = new();
        }

        public List<Skill> GetSkills()
        {
            List<Skill> skills = new();

            foreach (int skillId in SkillsId)
            {
                Skill? skill = DofusApi.Instance.Datacenter.SkillsData.GetSkillById(skillId);
                if (skill is not null)
                    skills.Add(skill);
            }

            return skills;
        }
    }

    public sealed class InteractiveObjectsData
    {
        private const string FILE_NAME = "interactiveobjects.json";

        [JsonPropertyName("IOg")]
        public List<InteractiveObjectGfx> InteractiveObjectsGfx { get; init; }

        [JsonPropertyName("IOd")]
        public List<InteractiveObject> InteractiveObjects { get; init; }

        public InteractiveObjectsData()
        {
            InteractiveObjectsGfx = new();
            InteractiveObjects = new();
        }

        internal static InteractiveObjectsData Build()
        {
            return Json.LoadFromFile<InteractiveObjectsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public InteractiveObject? GetInteractiveObjectById(int id)
        {
            return InteractiveObjects.Find(x => x.Id == id);
        }
    }
}
