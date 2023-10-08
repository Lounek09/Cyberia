using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record AddTTGCardToBinderEffect : Effect, IEffect<AddTTGCardToBinderEffect>
    {
        public int TTGCardId { get; init; }

        private AddTTGCardToBinderEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, int ttgCardId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            TTGCardId = ttgCardId;
        }

        public static AddTTGCardToBinderEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public TTGCardData? GetTTGCardData()
        {
            return DofusApi.Instance.Datacenter.TTGData.GetTTGCardDataById(TTGCardId);
        }

        public Description GetDescription()
        {
            TTGCardData? ttgCard = GetTTGCardData();
            string ttgEntityName = ttgCard is null ? $"{nameof(TTGCardData)} {PatternDecoder.Description(Resources.Unknown_Data, TTGCardId)}" : DofusApi.Instance.Datacenter.TTGData.GetTTGEntityNameById(ttgCard.TTGEntityId);

            return GetDescription(null, null, ttgEntityName);
        }
    }
}
