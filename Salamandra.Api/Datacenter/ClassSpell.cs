namespace Salamandra.Api.Datacenter
{
    [Table("ClassesSpells")]
    public sealed class ClassSpell
    {
        [PrimaryKey, NotNull]
        public int ClassId { get; set; }

        [PrimaryKey, NotNull]
        public int SpellId { get; set; }

        public ClassSpell()
        {

        }
    }
}
