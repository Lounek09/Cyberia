namespace Salamandra.Api.Datacenter
{
    [Table("Guild")]
    public sealed class Guild
    {
        [NotNull]
        public string WeightBoostCost { get; set; }

        [NotNull]
        public string ProspectingBoostCost { get; set; }

        [NotNull]
        public string TaxCollectorBoostCost { get; set; }

        [NotNull]
        public string WisdomBoostCost { get; set; }

        [NotNull]
        public string SpellLevelBoostCost { get; set; }

        [NotNull]
        public int ProspectingMax { get; set; }

        [NotNull]
        public int TaxCollectorMax { get; set; }

        [NotNull]
        public int WisdomMax { get; set; }

        [NotNull]
        public int SpellLevelMax { get; set; }

        public Guild()
        {
            WeightBoostCost = string.Empty;
            ProspectingBoostCost = string.Empty;
            TaxCollectorBoostCost = string.Empty;
            WisdomBoostCost = string.Empty;
            SpellLevelBoostCost = string.Empty;
        }
    }
}
