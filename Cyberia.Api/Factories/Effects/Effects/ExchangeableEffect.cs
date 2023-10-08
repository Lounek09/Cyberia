using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record ExchangeableEffect : Effect, IEffect<ExchangeableEffect>
    {
        public DateTime DateTime { get; init; }

        private ExchangeableEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, DateTime dateTime) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            DateTime = dateTime;
        }

        public static ExchangeableEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, DateManager.GetDateTimeFromEffectParameters(parameters));
        }

        public bool IsLinkedToAccount()
        {
            return DateTime == DateTime.MaxValue;
        }

        public Description GetDescription()
        {
            if (IsLinkedToAccount())
                return GetDescription(Resources.Effect_LinkedToAccount);

            return GetDescription(DateTime.ToString("dd/MM/yyy"));
        }
    }
}
