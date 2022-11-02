namespace Salamandra.Api.Datacenter
{
    [Table("SpellCategories")]
    public sealed class SpellCategory
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
