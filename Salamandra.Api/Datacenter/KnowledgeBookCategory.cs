namespace Salamandra.Api.Datacenter
{
    [Table("KnowledgeBookCategories")]
    public sealed class KnowledgeBookCategory
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int Order { get; set; }

        [NotNull]
        public int Index { get; set; }

        [NotNull]
        public int Episode { get; set; }

        public KnowledgeBookCategory()
        {
            Name = string.Empty;
        }
    }
}
