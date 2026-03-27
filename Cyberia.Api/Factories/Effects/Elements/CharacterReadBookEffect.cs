using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterReadBookEffect : Effect
{
    public int BookId { get; init; }

    private CharacterReadBookEffect(int id, int bookId)
        : base(id)
    {
        BookId = bookId;
    }

    internal static CharacterReadBookEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, BookId);
    }
}
