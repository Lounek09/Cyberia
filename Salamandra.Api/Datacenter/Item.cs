namespace Salamandra.Api.Datacenter
{
    [Table("Items")]
    public sealed class Item
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int ItemTypeId { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public int Episode { get; set; }

        [NotNull]
        public int GfxId { get; set; }

        [NotNull]
        public bool Wieldable { get; set; }

        [NotNull]
        public bool Enhanceable { get; set; }

        [NotNull]
        public int Weight { get; set; }

        [NotNull]
        public bool Ethereal { get; set; }

        [NotNull]
        public int AnimationId { get; set; }

        [NotNull]
        public bool TwoHanded { get; set; }

        [NotNull]
        public string WeaponInfos { get; set; }

        [NotNull]
        public string Criterion { get; set; }

        [NotNull]
        public int ItemSetId { get; set; }

        [NotNull]
        public bool Usable { get; set; }

        [NotNull]
        public bool Targetable { get; set; }

        [NotNull]
        public bool Cursed { get; set; }

        [NotNull]
        public bool Ceremonial { get; set; }

        [NotNull]
        public int Price { get; set; }

        [NotNull]
        public string Craft { get; set; }

        [NotNull]
        public string Stats { get; set; }

        public Item()
        {
            Name = string.Empty;
            Description = string.Empty;
            WeaponInfos = string.Empty;
            Criterion = string.Empty;
            Craft = string.Empty;
            Stats = string.Empty;
        }
    }
}
