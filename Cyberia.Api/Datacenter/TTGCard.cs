namespace Cyberia.Api.Datacenter
{
    [Table("TTGCards")]
    public sealed class TTGCard : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int Index { get; set; }

        [NotNull]
        public int ItemId { get; set; }

        [NotNull]
        public int TTGEntityId { get; set; }

        [NotNull]
        public int Variant { get; set; }

        public TTGCard()
        {

        }
    }
}
