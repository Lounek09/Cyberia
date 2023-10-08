using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record DisplayTitleEffect : Effect, IEffect<DisplayTitleEffect>
    {
        public int TitleId { get; init; }

        private DisplayTitleEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, int titleId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            TitleId = titleId;
        }

        public static DisplayTitleEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public TitleData? GetTitleData()
        {
            return DofusApi.Instance.Datacenter.TitlesData.GetTitleDataById(TitleId);
        }

        public Description GetDescription()
        {
            string titleName = DofusApi.Instance.Datacenter.TitlesData.GetTitleNameById(TitleId);

            return GetDescription(null, null, titleName);
        }
    }
}
