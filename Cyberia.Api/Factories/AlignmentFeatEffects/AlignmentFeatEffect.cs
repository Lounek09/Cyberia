using Cyberia.Api.Data.Alignments;
using Cyberia.Api.Factories.AlignmentFeatEffects.Elements;
using Cyberia.Langzilla.Enums;

using System.Globalization;

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

    public DescriptionString GetDescription(Language language)
    {
        return GetDescription(language.ToCulture());
    }

    public abstract DescriptionString GetDescription(CultureInfo? culture = null);

    /// <inheritdoc cref="IAlignmentFeatEffect.GetDescription"/>/>
    protected DescriptionString GetDescription(CultureInfo? culture, params int[] parameters)
    {
        var alignmentFeatEffect = GetAlignmentFeatEffectData();
        if (alignmentFeatEffect is null)
        {
            if (this is not UntranslatedAlignmentFeatEffect)
            {
                Log.Warning("Unknown AlignmentFeatEffectData {@AlignmentFeatEffect}", this);
            }

            return new DescriptionString(Translation.Get<ApiTranslations>("AlignmentFeatEffect.Unknown", culture),
                Id.ToString(),
                string.Join(',', parameters));
        }

        return new DescriptionString(alignmentFeatEffect.Description.ToString(culture), Array.ConvertAll(parameters, x => x.ToString()));
    }
}
