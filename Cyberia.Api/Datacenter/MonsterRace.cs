namespace Cyberia.Api.Datacenter
{
    [Table("MonsterRaces")]
    public sealed class MonsterRace : ITable
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
