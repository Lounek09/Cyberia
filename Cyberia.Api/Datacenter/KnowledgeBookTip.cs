namespace Cyberia.Api.Datacenter
{
    [Table("KnowledgeBookTips")]
    public sealed class KnowledgeBookTip : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int Index { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public int KnowledgeBookArticleId { get; set; }

        public KnowledgeBookTip()
        {
            Description = string.Empty;
        }
    }
}
