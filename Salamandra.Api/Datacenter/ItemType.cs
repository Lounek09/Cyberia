namespace Salamandra.Api.Datacenter
{
    [Table("ItemTypes")]
    public sealed class ItemType
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int ItemSuperTypeId { get; set; }

        [NotNull]
        public string EffectArea { get; set; }

        public ItemType()
        {
            Name = string.Empty;
            EffectArea = string.Empty;
        }
    }
}
