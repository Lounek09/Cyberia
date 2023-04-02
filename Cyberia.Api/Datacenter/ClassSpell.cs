namespace Cyberia.Api.Datacenter
{
    [Table("ClassesSpells")]
    public sealed class ClassSpell : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int ClassId { get; set; }

        [NotNull]
        public int SpellId { get; set; }

        public ClassSpell()
        {
            Id = string.Empty;
        }
    }
}
