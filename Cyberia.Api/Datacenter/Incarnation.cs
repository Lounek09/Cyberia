namespace Cyberia.Api.Datacenter
{
    [Table("Incarnations")]
    public sealed class Incarnation : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int GfxId { get; set; }

        [NotNull]
        public int ItemId { get; set; }

        [NotNull]
        public string Stats { get; set; }

        public Incarnation()
        {
            Name = string.Empty;
            Stats = string.Empty;
        }
    }
}
