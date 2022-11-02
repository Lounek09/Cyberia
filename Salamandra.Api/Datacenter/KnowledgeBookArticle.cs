namespace Salamandra.Api.Datacenter
{
    [Table("KnowledgeBookArticles")]
    public sealed class KnowledgeBookArticle
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int KnowledgeBookCategoryId { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int Order { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public string KeyWords { get; set; }

        [NotNull]
        public int Index { get; set; }

        [NotNull]
        public int Episode { get; set; }

        public KnowledgeBookArticle()
        {
            Name = string.Empty;
            Description = string.Empty;
            KeyWords = string.Empty;
        }
    }
}
