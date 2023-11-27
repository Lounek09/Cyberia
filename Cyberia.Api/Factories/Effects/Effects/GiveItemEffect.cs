using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Effects;

public sealed record GiveItemEffect : Effect, IEffect<GiveItemEffect>
{
    public int ItemId { get; init; }
    public int Qte { get; init; }
    public GiveItemTarget Target { get; init; }

    private GiveItemEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int itemId, int qte, GiveItemTarget giveItemTarget)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        ItemId = itemId;
        Qte = qte;
        Target = giveItemTarget;
    }

    public static GiveItemEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3, parameters.Param2, (GiveItemTarget)parameters.Param1);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemDataById(ItemId);
    }

    public Description GetDescription()
    {
        var itemName = DofusApi.Datacenter.ItemsData.GetItemNameById(ItemId);

        return GetDescription(Target.GetDescription(), Qte, itemName);
    }
}
