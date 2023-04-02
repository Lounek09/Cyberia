namespace Cyberia.Api.Datacenter
{
    [Table("Quests")]
    public sealed class Quest : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        public bool IsRepeatable { get; set; }

        public bool IsAccount { get; set; }

        public bool HasDungeon { get; set; }

        public Quest()
        {
            Name = string.Empty;
        }
    }
}
