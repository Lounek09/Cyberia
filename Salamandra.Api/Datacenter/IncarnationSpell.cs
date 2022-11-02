namespace Salamandra.Api.Datacenter
{
    [Table("IncarnationsSpells")]
    public sealed class IncarnationSpell
    {
        [PrimaryKey, NotNull]
        public int IncarnationId { get; set; }

        [PrimaryKey, NotNull]
        public int SpellId { get; set; }

        public IncarnationSpell()
        {

        }
    }
}
