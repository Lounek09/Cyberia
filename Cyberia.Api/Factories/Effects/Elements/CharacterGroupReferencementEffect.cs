using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterGroupReferencementEffect : Effect
{
    public JobGroup JobGroup { get; init; }

    private CharacterGroupReferencementEffect(int id, JobGroup jobGroup)
        : base(id)
    {
        JobGroup = jobGroup;
    }

    internal static CharacterGroupReferencementEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (JobGroup)parameters.Param1);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, JobGroup.GetDescription(culture));
    }
}
