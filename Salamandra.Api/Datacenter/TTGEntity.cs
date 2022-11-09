namespace Salamandra.Api.Datacenter
{
    [Table("TTGEntities")]
    public sealed class TTGEntity
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int Index { get; set; }

        [NotNull]
        public int Rariry { get; set; }

        [NotNull]
        public int TTGFamilyId { get; set; }

        [NotNull]
        public string Name { get; set; }

        public TTGEntity()
        {
            Name = string.Empty;
        }
    }
}
