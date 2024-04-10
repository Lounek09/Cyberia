using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

[JsonConverter(typeof(ItemWeaponDataConverter))]
public sealed class ItemWeaponData : IDofusData
{
    public int CriticalBonus { get; init; }

    public int ActionPointCost { get; init; }

    public int MinRange { get; init; }

    public int MaxRange { get; init; }

    public int CriticalHitRate { get; init; }

    public int CriticalFailureRate { get; init; }

    public bool LineOnly { get; init; }

    public bool LineOfSight { get; init; }

    internal ItemWeaponData()
    {

    }
}
