namespace Cyberia.Api.Datacenter
{
    [Table("TaxCollectorLastNames")]
    public sealed class TaxCollectorLastName
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        public TaxCollectorLastName()
        {
            Name = string.Empty;
        }
    }
}
