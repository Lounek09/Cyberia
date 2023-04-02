namespace Cyberia.Api.Datacenter
{
    [Table("SpellCategories")]
    public sealed class SpellCategory : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        public SpellCategory()
        {
            Name = string.Empty;
        }
    }
}
