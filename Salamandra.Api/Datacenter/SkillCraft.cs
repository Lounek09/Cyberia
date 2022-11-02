namespace Salamandra.Api.Datacenter
{
    [Table("SkillsCrafts")]
    public sealed class SkillCraft
    {
        [PrimaryKey, NotNull]
        public int SkillId { get; set; }

        [PrimaryKey, NotNull]
        public int ItemId { get; set; }

        public SkillCraft()
        {

        }
    }
}
