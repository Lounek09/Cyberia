using Cyberia.Api.Data.Alignments;

namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public abstract record AlignmentFeatEffect(int Id)
{
    public AlignmentFeatEffectData? GetAlignmentFeatEffectData()
    {
        return DofusApi.Datacenter.AlignmentsData.GetAlignmentFeatEffectDataById(Id);
    }

    protected Description GetDescription(params int[] parameters)
    {
        var alignmentFeatEffect = GetAlignmentFeatEffectData();
        if (alignmentFeatEffect is null)
        {
            Log.Information("Unknown AlignmentFeatEffectData {@AlignmentFeatEffect}", this);
            return new(Resources.AlignmentFeatEffect_Unknown,
                Id.ToString(),
                string.Join(',', parameters));
        }

        return new(alignmentFeatEffect.Description, Array.ConvertAll(parameters, x => x.ToString()));
    }
}
