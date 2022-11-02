namespace Salamandra.Api.Datacenter
{
    [Table("Runes")]
    public sealed class Rune
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public decimal Weight { get; set; }

        [NotNull]
        public int Power { get; set; }

        [NotNull]
        public bool HasPa { get; set; }

        [NotNull]
        public bool HasRa { get; set; }

        public Rune()
        {
            Name = string.Empty;
        }
    }
}
