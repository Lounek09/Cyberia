using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record ShowTitleEffect : BasicEffect
    {
        public int TitleId { get; init; }

        public ShowTitleEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            TitleId = parameters.Param3;
        }

        public static new ShowTitleEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public Title? GetTitle()
        {
            return DofusApi.Instance.Datacenter.TitlesData.GetTitleById(TitleId);
        }

        public override string GetDescription()
        {
            string titleName = DofusApi.Instance.Datacenter.TitlesData.GetTitleNameById(TitleId);

            return GetDescriptionFromParameters(null, null, titleName);
        }
    }
}
