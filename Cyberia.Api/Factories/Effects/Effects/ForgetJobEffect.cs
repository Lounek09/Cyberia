using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record ForgetJobEffect : Effect, IEffect<ForgetJobEffect>
    {
        public int JobId { get; init; }

        private ForgetJobEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int jobId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            JobId = jobId;
        }

        public static ForgetJobEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public JobData? GetJobData()
        {
            return DofusApi.Instance.Datacenter.JobsData.GetJobDataById(JobId);
        }

        public Description GetDescription()
        {
            string jobName = DofusApi.Instance.Datacenter.JobsData.GetJobNameById(JobId);

            return GetDescription(null, null, jobName);
        }
    }
}
