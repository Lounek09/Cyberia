using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class AddTTGCardToBinderEffect : BasicEffect
    {
        public int TTGCardId { get; init; }

        public AddTTGCardToBinderEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) : 
            base(effectId, parameters, duration, probability, area)
        {
            TTGCardId = parameters.Param3;
        }

        public static new AddTTGCardToBinderEffect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
        {
            return new(effectId, parameters, duration, probability, area);
        }

        public TTGCard? GetTTGCard()
        {
            return DofusApi.Instance.Datacenter.TTGData.GetTTGCardById(TTGCardId);
        }

        public override string GetDescription()
        {
            TTGCard? ttgCard = GetTTGCard();

            string ttgEntityName = ttgCard is null ? $"Carte inconnu ({TTGCardId})" : DofusApi.Instance.Datacenter.TTGData.GetTTGEntityNameById(ttgCard.TTGEntityId);

            return GetDescriptionFromParameters(null, null, ttgEntityName);
        }
    }
}
