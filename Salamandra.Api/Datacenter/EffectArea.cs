namespace Salamandra.Api.Datacenter
{
    [Table("EffectAreas")]
    public sealed class EffectArea
    {
        [PrimaryKey, NotNull]
        public string Symbol { get; set; }

        [NotNull]
        public string Name { get; set; }

        public EffectArea()
        {
            Symbol = string.Empty;
            Name = string.Empty;
        }
    }
}
