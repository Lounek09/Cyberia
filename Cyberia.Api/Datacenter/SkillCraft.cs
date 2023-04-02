namespace Cyberia.Api.Datacenter
{
    [Table("SkillsCrafts")]
    public sealed class SkillCraft : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int SkillId { get; set; }

        [NotNull]
        public int ItemId { get; set; }

        public SkillCraft()
        {
            Id = string.Empty;
        }
    }
}
