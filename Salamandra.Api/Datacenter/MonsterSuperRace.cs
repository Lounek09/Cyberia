namespace Salamandra.Api.Datacenter
{
    [Table("MonsterSuperRaces")]
    public sealed class MonsterSuperRace
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        public MonsterSuperRace()
        {
            Name = string.Empty;
        }
    }
}
