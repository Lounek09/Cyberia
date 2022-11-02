namespace Salamandra.Api.Datacenter
{
    [Table("HintCategories")]
    public sealed class HintCategory
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Color { get; set; }

        public HintCategory()
        {
            Name = string.Empty;
            Color = string.Empty;
        }
    }
}
