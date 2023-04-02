namespace Cyberia.Api.Datacenter
{
    [Table("Spells")]
    public sealed class Spell : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Description { get; set; }

        public Spell()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }
}
