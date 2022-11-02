namespace Salamandra.Api.Datacenter
{
    [Table("MonsterRaces")]
    public sealed class MonsterRace
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int MonsterSuperRaceId { get; set; }

        public MonsterRace()
        {
            Name = string.Empty;
        }
    }
}
