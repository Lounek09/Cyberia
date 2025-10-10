using Cyberia.Api.Data.Items;
using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Common;

[JsonConverter(typeof(AccessoriesDataConverter))]
public sealed class AccessoriesData : IDofusData
{
    public int WeaponItemId { get; init; }

    public int HatItemId { get; init; }

    public int CloakItemId { get; init; }

    public int PetItemId { get; init; }

    public int ShieldItemId { get; init; }

    [JsonConstructor]
    internal AccessoriesData() { }

    public ItemData? GetWeaponItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(WeaponItemId);
    }

    public ItemData? GetHatItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(HatItemId);
    }

    public ItemData? GetCloakItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(CloakItemId);
    }

    public ItemData? GetPetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(PetItemId);
    }

    public ItemData? GetShieldItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ShieldItemId);
    }
}
