using Cyberia.Api.Data.Jobs;

namespace Cyberia.Api.Factories.Effects.Templates;

public interface IJobEffect
{
    public int JobId { get; init; }

    public JobData? GetJobData();
}
