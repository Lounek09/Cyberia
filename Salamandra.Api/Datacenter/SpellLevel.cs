namespace Salamandra.Api.Datacenter
{
    [Table("SpellLevels")]
    public sealed class SpellLevel
    {
        [PrimaryKey, NotNull]
        public int SpellId { get; set; }

        [PrimaryKey, NotNull]
        public int Level { get; set; }

        [NotNull]
        public string Effects { get; set; }

        [NotNull]
        public string CriticalEffects { get; set; }

        [NotNull]
        public int ApCost { get; set; }

        [NotNull]
        public int RangeMin { get; set; }

        [NotNull]
        public int RangeMax { get; set; }

        [NotNull]
        public int CriticalHitRate { get; set; }

        [NotNull]
        public int CriticalFailureRate { get; set; }

        [NotNull]
        public bool IsLineOnly { get; set; }

        [NotNull]
        public bool HasLineOfSight { get; set; }

        [NotNull]
        public bool NeedFreeCell { get; set; }

        [NotNull]
        public bool CanBoostRange { get; set; }

        [NotNull]
        public int SpellCategoryId { get; set; }

        [NotNull]
        public int LaunchCountByTurn { get; set; }

        [NotNull]
        public int LaunchCountByPlayerByTurn { get; set; }

        [NotNull]
        public int DelayBetweenLaunch { get; set; }

        [NotNull]
        public string EffectsArea { get; set; }

        [NotNull]
        public int NeededLevel { get; set; }

        [NotNull]
        public bool IsCricalFailureEndTheTurn { get; set; }

        public SpellLevel()
        {
            Effects = string.Empty;
            CriticalEffects = string.Empty;
            EffectsArea = string.Empty;
        }
    }
}
