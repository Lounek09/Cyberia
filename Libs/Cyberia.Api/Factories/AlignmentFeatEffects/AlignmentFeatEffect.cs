using Cyberia.Api.Data.Alignments;
using Cyberia.Api.Factories.AlignmentFeatEffects.Elements;
using Cyberia.Langzilla.Primitives;

using System.Globalization;
using System.Runtime.CompilerServices;

namespace Cyberia.Api.Factories.AlignmentFeatEffects;

/// <summary>
/// Represents an effect of an alignment feat.
/// </summary>
public abstract record AlignmentFeatEffect
{
    /// <summary>
    /// Gets the unique identifier of the alignment feat effect.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlignmentFeatEffect"/> record.
    /// </summary>
    /// <param name="id">The unique identifier of the alignment feat effect.</param>
    protected AlignmentFeatEffect(int id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets the alignment feat effect data.
    /// </summary>
    /// <returns>The <see cref="AlignmentFeatEffectData"/> object containing the data of the alignment feat effect.</returns>
    public AlignmentFeatEffectData? GetData()
    {
        return DofusApi.Datacenter.AlignmentsRepository.GetAlignmentFeatEffectDataById(Id);
    }

    /// <summary>
    /// Generates a human-readable description of the alignment feat effect for the specified language.
    /// </summary>
    /// <param name="language">The language to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the alignment feat effect for the specified language.</returns>
    public DescriptionString GetDescription(Language language)
    {
        return GetDescription(language.ToCulture());
    }

    /// <summary>
    /// Generates a human-readable description of the alignment feat effect for the specified culture.
    /// </summary>
    /// <param name="culture">The culture to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the alignment feat effect for the specified culture.</returns>
    [OverloadResolutionPriority(2)]
    public abstract DescriptionString GetDescription(CultureInfo? culture = null);

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    [OverloadResolutionPriority(1)]
    protected DescriptionString GetDescription(CultureInfo? culture, params int[] parameters)
    {
        var alignmentFeatEffect = GetData();
        if (alignmentFeatEffect is null)
        {
            if (this is not UntranslatedAlignmentFeatEffect)
            {
                Log.Warning("Unknown AlignmentFeatEffectData {@AlignmentFeatEffect}", this);
            }

            return new DescriptionString(Translation.Get<ApiTranslations>("AlignmentFeatEffect.Unknown", culture),
                Id.ToString(), string.Join(',', parameters));
        }

        return new DescriptionString(alignmentFeatEffect.Description.ToString(culture), Array.ConvertAll(parameters, x => x.ToString()));
    }
}
