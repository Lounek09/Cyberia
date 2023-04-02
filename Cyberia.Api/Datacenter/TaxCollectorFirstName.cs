namespace Cyberia.Api.Datacenter
{
    [Table("TaxCollectorFirstNames")]
    public sealed class TaxCollectorFirstName
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        public TaxCollectorFirstName()
        {
            Name = string.Empty;
        }
    }
}
