namespace Cyberia.Api.Datacenter
{
    [Table("KnowledgeBookTriggers")]
    public sealed class KnowledgeBookTrigger : ITable
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
