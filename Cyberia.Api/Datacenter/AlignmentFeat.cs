﻿namespace Cyberia.Api.Datacenter
{
    [Table("AlignmentFeats")]
    public sealed class AlignmentFeat : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int GfxId { get; set; }

        [NotNull]
        public int AlignmentFeatEffectId { get; set; }

        public AlignmentFeat()
        {
            Name = string.Empty;
        }
    }
}