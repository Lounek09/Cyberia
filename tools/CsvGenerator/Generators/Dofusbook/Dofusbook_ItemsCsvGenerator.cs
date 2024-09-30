using CsvGenerator.Extensions;

using Cyberia.Api.Data.Items;
using Cyberia.Api.Enums;

namespace CsvGenerator.Generators.Dofusbook;

public sealed class Dofusbook_ItemsCsvGenerator : DofusCsvGenerator<ItemData>
{
    private static readonly IReadOnlyList<string> s_columns =
    [
        "name",
        "official",
        "cloth_id",
        "picture",
        "category",
        "level",
        "name_fr",
        "name_en",
        "name_es",
        "desc_fr",
        "desc_en",
        "desc_es",
        "effect",
        "constraint",
        "pa",
        "po_min",
        "po_max",
        "bonus_cc",
        "cc",
        "ec",
        "one_hand",
        "ceremonial",
        "craft"
    ];

    private static readonly IReadOnlyList<ItemSuperType> s_filteredItemSuperType =
    [
        ItemSuperType.None,
        ItemSuperType.Quest,
        ItemSuperType.Mutation,
        ItemSuperType.FoodBoost,
        ItemSuperType.Blessing,
        ItemSuperType.Curse,
        ItemSuperType.RoleplayBuff,
        ItemSuperType.FollowingCharacter,
        ItemSuperType.LivingItem,
        ItemSuperType.FullSoulStone,
        ItemSuperType.TTGCard,
        ItemSuperType.Tonic
    ];

    private static readonly IReadOnlyList<int> s_filteredItemType =
    [
        ItemTypeData.BowKennelCertificate,
        ItemTypeData.Gifts,
        ItemTypeData.ShushuSoulFragment
    ];

    public override string Name => "dofusbook_items";

    public Dofusbook_ItemsCsvGenerator(IEnumerable<ItemData> items)
        : base(s_columns, items)
    {

    }

    protected override IEnumerable<ItemData> FilteredItems()
    {
        return _items.Where(x =>
        {
            var itemTypeData = x.GetItemTypeData();
            if (itemTypeData is null)
            {
                return false;
            }

            return !s_filteredItemSuperType.Contains(itemTypeData.ItemSuperType) &&
                !s_filteredItemType.Contains(itemTypeData.Id);
        });
    }

    protected override void AppendItem(ItemData item)
    {
        //name
        _builder.AppendCsvString(item.Name);
        _builder.Append(c_csvSeparator);

        //official
        _builder.Append(item.Id);
        _builder.Append(c_csvSeparator);

        //cloth_id
        if (item.ItemSetId != 0)
        {
            _builder.Append(item.ItemSetId);
        }
        _builder.Append(c_csvSeparator);

        //picture
        _builder.Append(item.ItemTypeId);
        _builder.Append('/');
        _builder.Append(item.GfxId);
        _builder.Append(c_csvSeparator);

        //category
        var itemTypeData = item.GetItemTypeData();
        if (itemTypeData is not null)
        {
            _builder.AppendCsvString(itemTypeData.Name);
        }
        _builder.Append(c_csvSeparator);

        //level
        _builder.Append(item.Level);
        _builder.Append(c_csvSeparator);

        //name_fr
        _builder.AppendCsvString(item.Name.ToString("fr"));
        _builder.Append(c_csvSeparator);

        //name_en
        _builder.AppendCsvString(item.Name.ToString("en"));
        _builder.Append(c_csvSeparator);

        //name_es
        _builder.AppendCsvString(item.Name.ToString("es"));
        _builder.Append(c_csvSeparator);

        //desc_fr
        _builder.AppendCsvString(item.Description.ToString("fr"));
        _builder.Append(c_csvSeparator);

        //desc_en
        _builder.AppendCsvString(item.Description.ToString("en"));
        _builder.Append(c_csvSeparator);

        //desc_es
        _builder.AppendCsvString(item.Description.ToString("es"));
        _builder.Append(c_csvSeparator);

        //effect
        var itemStatData = item.GetItemStatsData();
        if (itemStatData is not null)
        {
            _builder.AppendEffects(itemStatData.Effects);
        }
        _builder.Append(c_csvSeparator);

        //constraint
        _builder.AppendCriteria(item.Criteria);
        _builder.Append(c_csvSeparator);

        var weaponData = item.WeaponData;

        //pa
        _builder.Append(weaponData?.ActionPointCost);
        _builder.Append(c_csvSeparator);

        //po_min
        _builder.Append(weaponData?.MinRange);
        _builder.Append(c_csvSeparator);

        //po_max
        _builder.Append(weaponData?.MaxRange);
        _builder.Append(c_csvSeparator);

        //bonus_cc
        _builder.Append(weaponData?.CriticalBonus);
        _builder.Append(c_csvSeparator);

        //cc
        _builder.Append(weaponData?.CriticalHitRate);
        _builder.Append(c_csvSeparator);

        //ec
        _builder.Append(weaponData?.CriticalFailureRate);
        _builder.Append(c_csvSeparator);

        //one_hand
        if (!item.TwoHanded)
        {
            _builder.Append('x');
        }
        _builder.Append(c_csvSeparator);

        //ceremonial
        if (item.Ceremonial)
        {
            _builder.Append('x');
        }
        _builder.Append(c_csvSeparator);

        //craft
        var craftData = item.GetCraftData();
        if (craftData is not null)
        {
            _builder.AppendCraft(craftData);
        }
        _builder.AppendLine();
    }
}
