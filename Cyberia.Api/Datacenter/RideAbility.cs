namespace Cyberia.Api.Datacenter
{
    [Table("RideAbilities")]
    public sealed class RideAbility : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Description { get; set; }

        public RideAbility()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }
}
