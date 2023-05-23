using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record GiveItemEffect : BasicEffect
    {
        public int ItemId { get; init; }
        public int Qte { get; init; }
        public string Target { get; init; }

        public GiveItemEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            ItemId = parameters.Param3;
            Qte = parameters.Param2;
            Target = parameters.Param1 == 0 ? "cible, check cible" : parameters.Param1 == 1 ? "lanceur, check lanceur" : parameters.Param1 == 2 ? "lanceur, check cible" : parameters.Param1.ToString();
        }

        public static new GiveItemEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public Item? GetItem()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemById(ItemId);
        }

        public override string GetDescription()
        {
            string itemName = DofusApi.Instance.Datacenter.ItemsData.GetItemNameById(ItemId).SanitizeMarkDown();

            return GetDescriptionFromParameters(Target, Qte.ToString(), itemName);
        }
    }
}
