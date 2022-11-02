namespace Salamandra.Api.Datacenter
{
    [Table("Effects")]
    public sealed class Effect
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public int CharacteristicId { get; set; }

        [NotNull]
        public string Operator { get; set; }

        [NotNull]
        public bool ShowInTooltip { get; set; }

        [NotNull]
        public bool ShowInDiceModePossible { get; set; }

        [NotNull]
        public string Element { get; set; }

        [NotNull]
        public bool IsDamagingEffect { get; set; }

        [NotNull]
        public bool IsHealingEffect { get; set; }

        public Effect()
        {
            Description = string.Empty;
            Operator = string.Empty;
            Element = string.Empty;
        }
    }
}
