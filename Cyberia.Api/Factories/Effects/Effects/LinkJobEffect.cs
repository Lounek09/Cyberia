using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record LinkJobEffect : Effect, IEffect<LinkJobEffect>
    {
        public int JobId { get; init; }

        private LinkJobEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, int jobId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            JobId = jobId;
        }

        public static LinkJobEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param1);
        }

        public JobData? GetJobData()
        {
            return DofusApi.Instance.Datacenter.JobsData.GetJobDataById(JobId);
        }

        public Description GetDescription()
        {
            string jobName = DofusApi.Instance.Datacenter.JobsData.GetJobNameById(JobId);

            return GetDescription(jobName);
        }
    }
}
