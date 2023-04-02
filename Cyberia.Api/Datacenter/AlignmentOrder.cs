namespace Cyberia.Api.Datacenter
{
    [Table("AlignmentOrders")]
    public sealed class AlignmentOrder : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int AlignmentId { get; set; }

        public AlignmentOrder()
        {
            Name = string.Empty;
        }
    }
}
