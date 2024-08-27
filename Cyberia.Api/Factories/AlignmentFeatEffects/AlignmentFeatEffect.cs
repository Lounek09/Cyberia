using Cyberia.Api.Data.Alignments;

namespace Cyberia.Api.Factories.AlignmentFeatEffects;

/// <inheritdoc cref="IAlignmentFeatEffect"/>/>
public abstract record AlignmentFeatEffect : IAlignmentFeatEffect
{
    public int Id { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlignmentFeatEffect"/> record.
    /// </summary>
    /// <param name="id">The unique identifier of the alignment feat effect.</param>
    protected AlignmentFeatEffect(int id)
    {
        Id = id;
    }

    public AlignmentFeatEffectData? GetAlignmentFeatEffectData()
    {
        return DofusApi.Datacenter.AlignmentsRepository.GetAlignmentFeatEffectDataById(Id);
    }

    public abstract DescriptionString GetDescription();

    /// <inheritdoc cref="IAlignmentFeatEffect.GetDescription"/>/>
    protected DescriptionString GetDescription(params int[] parameters)
    {
        var alignmentFeatEffect = GetAlignmentFeatEffectData();
        if (alignmentFeatEffect is null)
        {
            Log.Information("Unknown AlignmentFeatEffectData {@AlignmentFeatEffect}", this);
            return new(ApiTranslations.AlignmentFeatEffect_Unknown,
                Id.ToString(),
                string.Join(',', parameters));
        }

        return new(alignmentFeatEffect.Description, Array.ConvertAll(parameters, x => x.ToString()));
    }
}
