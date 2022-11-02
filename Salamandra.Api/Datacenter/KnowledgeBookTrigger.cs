namespace Salamandra.Api.Datacenter
{
    [Table("KnowledgeBookTrigger")]
    public sealed class KnowledgeBookTrigger
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int Type { get; set; }

        [NotNull]
        public string Values { get; set; }

        [NotNull]
        public int KnowledgeBookTipId { get; set; }

        public KnowledgeBookTrigger()
        {
            Values = string.Empty;
        }
    }
}
