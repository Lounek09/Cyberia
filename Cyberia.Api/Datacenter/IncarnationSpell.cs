namespace Cyberia.Api.Datacenter
{
    [Table("IncarnationsSpells")]
    public sealed class IncarnationSpell : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int IncarnationId { get; set; }

        [NotNull]
        public int SpellId { get; set; }

        public IncarnationSpell()
        {
            Id = string.Empty;
        }
    }
}
