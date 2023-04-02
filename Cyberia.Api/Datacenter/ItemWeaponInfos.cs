namespace Cyberia.Api.Datacenter
{
    [Table("ItemWeaponsInfos")]
    public sealed class ItemWeaponInfos
    {
        [PrimaryKey, NotNull]
        public int ItemId { get; set; }

        [NotNull]
        public int CriticalBonus { get; set; }

        [NotNull]
        public int ActionPointCost { get; set; }

        [NotNull]
        public int MinRange { get; set; }

        [NotNull]
        public int MaxRange { get; set; }

        [NotNull]
        public int CriticalRate { get; set; }

        [NotNull]
        public int CriticalFailureRate { get; set; }

        [NotNull]
        public bool InLine { get; set; }

        [NotNull]
        public bool LineOfSight { get; set; }

        public ItemWeaponInfos()
        {

        }
    }
}
