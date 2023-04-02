namespace Cyberia.Api.Datacenter
{
    [Table("InteractiveObjectsSkills")]
    public sealed class InteractiveObjectSkill : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int InteractiveObjectId { get; set; }

        [NotNull]
        public int SkillId { get; set; }

        public InteractiveObjectSkill()
        {
            Id = string.Empty;
        }
    }
}
