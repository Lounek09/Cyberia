namespace Cyberia.Api.Datacenter
{
    [Table("ItemSuperTypes")]
    public sealed class ItemSuperType : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public bool SuperTypeText { get; set; }

        [NotNull]
        public string Slots { get; set; }

        public ItemSuperType()
        {
            Name = string.Empty;
            Slots = string.Empty;
        }
    }
}
