namespace Salamandra.Api.Datacenter
{
    [Table("Alignments")]
    public sealed class Alignment
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public bool CanConquest { get; set; }

        public Alignment()
        {
            Name = string.Empty;
        }
    }
}
