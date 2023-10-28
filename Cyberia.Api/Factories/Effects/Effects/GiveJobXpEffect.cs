using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record GiveJobXpEffect : Effect, IEffect<GiveJobXpEffect>
    {
        public int JobId { get; init; }
        public int XpAmount { get; init; }

        private GiveJobXpEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int jobId, int xpAmount) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            JobId = jobId;
            XpAmount = xpAmount;
        }

        public static GiveJobXpEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param2, parameters.Param3);
        }

        public JobData? GetJobData()
        {
            return DofusApi.Instance.Datacenter.JobsData.GetJobDataById(JobId);
        }

        public Description GetDescription()
        {
            string jobName = DofusApi.Instance.Datacenter.JobsData.GetJobNameById(JobId);

            return GetDescription(XpAmount, jobName);
        }
    }
}
