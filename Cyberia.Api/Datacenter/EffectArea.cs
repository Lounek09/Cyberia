namespace Cyberia.Api.Datacenter
{
    [Table("EffectAreas")]
    public sealed class EffectArea : ITable
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
