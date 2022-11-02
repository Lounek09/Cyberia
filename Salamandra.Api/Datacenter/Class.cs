namespace Salamandra.Api.Datacenter
{
    [Table("Classes")]
    public sealed class Class
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string LongName { get; set; }

        [NotNull]
        public int Episode { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public string ShortDescription { get; set; }

        [NotNull]
        public string TemporisPassiveName { get; set; }

        [NotNull]
        public string TemporisPassiveDescription { get; set; }

        [NotNull]
        public string VitalityBoostCost { get; set; }

        [NotNull]
        public string WisdomBoostCost { get; set; }

        [NotNull]
        public string StrengthBoostCost { get; set; }

        [NotNull]
        public string IntelligenceBoostCost { get; set; }

        [NotNull]
        public string ChanceBoostCost { get; set; }

        [NotNull]
        public string AgilityBoostCost { get; set; }

        public int? ClassSpellId { get; set; }

        public Class()
        {
            Name = string.Empty;
            LongName = string.Empty;
            Description = string.Empty;
            ShortDescription = string.Empty;
            TemporisPassiveName = string.Empty;
            TemporisPassiveDescription = string.Empty;
            VitalityBoostCost = string.Empty;
            WisdomBoostCost = string.Empty;
            StrengthBoostCost = string.Empty;
            IntelligenceBoostCost = string.Empty;
            ChanceBoostCost = string.Empty;
            AgilityBoostCost = string.Empty;
        }
    }
}
