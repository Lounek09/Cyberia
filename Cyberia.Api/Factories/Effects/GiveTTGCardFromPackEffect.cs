using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class GiveTTGCardFromPackEffect : BasicEffect
    {
        public int TTGFamilyId { get; init; }

        public GiveTTGCardFromPackEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) : base(effectId, parameters, duration, probability, criteria, area)
        {
            TTGFamilyId = parameters.Param3;
        }

        public static new GiveTTGCardFromPackEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public TTGFamily? GetTTGFamily()
        {
            return DofusApi.Instance.Datacenter.TTGData.GetTTGFamilyById(TTGFamilyId);
        }

        public override string GetDescription()
        {
            string ttgFamilyName = DofusApi.Instance.Datacenter.TTGData.GetTTGFamilyNameById(TTGFamilyId);

            return GetDescriptionFromParameters(null, null, ttgFamilyName);
        }
    }
}
