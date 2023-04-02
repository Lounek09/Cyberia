namespace Cyberia.Api.Datacenter
{
    [Table("Monsters")]
    public sealed class Monster : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int GfxId { get; set; }

        [NotNull]
        public int MonsterRaceId { get; set; }

        [NotNull]
        public int AlignmentId { get; set; }

        [NotNull]
        public bool Kickable { get; set; }

        [NotNull]
        public bool Boss { get; set; }

        [NotNull]
        public bool VisibleInBigStoreSearch { get; set; }

        public Monster()
        {
            Name = string.Empty;
        }
    }
}
