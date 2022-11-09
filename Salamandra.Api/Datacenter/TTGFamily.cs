namespace Salamandra.Api.Datacenter
{
    [Table("TTGFamilies")]
    public sealed class TTGFamily
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int Index { get; set; }

        [NotNull]
        public int ItemId { get; set; }

        [NotNull]
        public string Name { get; set; }

        public TTGFamily()
        {
            Name = string.Empty;
        }
    }
}
