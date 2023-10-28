using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record GiveTTGCardEffect : Effect, IEffect<GiveTTGCardEffect>
    {
        public int TTGFamilyId { get; init; }

        private GiveTTGCardEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int ttgFamilyId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            TTGFamilyId = ttgFamilyId;
        }

        public static GiveTTGCardEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public TTGFamilyData? GetTTGFamilyData()
        {
            return DofusApi.Instance.Datacenter.TTGData.GetTTGFamilyDataById(TTGFamilyId);
        }

        public Description GetDescription()
        {
            string ttgFamilyName = DofusApi.Instance.Datacenter.TTGData.GetTTGFamilyNameById(TTGFamilyId);

            return GetDescription(null, null, ttgFamilyName);
        }
    }
}
