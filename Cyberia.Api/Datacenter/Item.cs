﻿namespace Cyberia.Api.Datacenter
{
    [Table("Items")]
    public sealed class Item : ITable
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
        public int Level { get; set; }

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

        public Item()
        {
            Name = string.Empty;
            Description = string.Empty;
            Criterion = string.Empty;
        }
    }
}