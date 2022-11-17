namespace Salamandra.Api.Datacenter
{
    [Table("Monsters")]
    public sealed class Monster
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int GfxId { get; set; }

        [NotNull]
        public int MonsterRace { get; set; }

        [NotNull]
        public int AlignmentId { get; set; }

        [NotNull]
        public bool Kickable { get; set; }

        [NotNull]
        public bool IsBoss { get; set; }

        [NotNull]
        public bool IsVisibleInBigStoreSearch { get; set; }

        public Monster()
        {
            Name = string.Empty;
        }
    }
}
