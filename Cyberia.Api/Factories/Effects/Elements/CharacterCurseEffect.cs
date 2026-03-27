using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterCurseEffect : Effect
{
    public int CurseId { get; init; }

    private CharacterCurseEffect(int id, int curseId)
        : base(id)
    {
        CurseId = curseId;
    }

    internal static CharacterCurseEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, CurseId);
    }
}
