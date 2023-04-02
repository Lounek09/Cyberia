namespace Cyberia.Api.Datacenter
{
    [Table("NpcsNpcActions")]
    public sealed class NpcNpcAction
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int NpcId { get; set; }

        [NotNull]
        public int NpcActionId { get; set; }

        public NpcNpcAction()
        {
            Id = string.Empty;
        }
    }
}
