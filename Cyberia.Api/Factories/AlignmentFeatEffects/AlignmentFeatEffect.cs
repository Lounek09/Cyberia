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
            var commaSeparatedParameters = string.Join(',', parameters);

            Log.Information("Unknown {AlignmentFeatEffectData} {AlignmentFeatEffectId} ({AlignmentFeatEffectParameters})",
                nameof(AlignmentFeatEffectData),
                Id,
                commaSeparatedParameters);

            return new(Resources.AlignmentFeatEffect_Unknown, Id.ToString(), commaSeparatedParameters);
        }

        return new(alignmentFeatEffect.Description, Array.ConvertAll(parameters, x => x.ToString()));
    }
}
