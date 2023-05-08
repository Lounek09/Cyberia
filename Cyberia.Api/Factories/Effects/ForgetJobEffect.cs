using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class ForgetJobEffect : BasicEffect
    {
        public int JobId { get; init; }

        public ForgetJobEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) : 
            base(effectId, parameters, duration, probability, area)
        {
            JobId = parameters.Param3;
        }

        public static new ForgetJobEffect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
        {
            return new(effectId, parameters, duration, probability, area);
        }

        public Job? GetJob()
        {
            return DofusApi.Instance.Datacenter.JobsData.GetJobById(JobId);
        }

        public override string GetDescription()
        {
            string jobName = DofusApi.Instance.Datacenter.JobsData.GetJobNameById(JobId);

            return GetDescriptionFromParameters(null, null, jobName);
        }
    }
}
