namespace Salamandra.Api.Datacenter
{
    [Table("Quests")]
    public sealed class Quest
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public bool IsRepeatable { get; set; }

        [NotNull]
        public bool IsAccount { get; set; }

        [NotNull]
        public bool HasDungeon { get; set; }

        public Quest()
        {
            Name = string.Empty;
        }
    }
}
