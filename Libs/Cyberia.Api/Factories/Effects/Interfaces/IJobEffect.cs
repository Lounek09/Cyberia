using Cyberia.Api.Data.Jobs;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IJobEffect
{
    public int JobId { get; }

    public JobData? GetJobData();
}
