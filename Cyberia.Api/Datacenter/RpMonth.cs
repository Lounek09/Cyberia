﻿namespace Cyberia.Api.Datacenter
{
    [Table("RpMonths")]
    public sealed class RpMonth : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        public RpMonth()
        {
            Name = string.Empty;
        }
    }
}