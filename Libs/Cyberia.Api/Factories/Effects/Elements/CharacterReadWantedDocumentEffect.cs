using Cyberia.Api.Data.WantedDocuments;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterReadWantedDocumentEffect : Effect, IWantedDocumentEffect
{
    public int WantedDocumentId { get; init; }

    private CharacterReadWantedDocumentEffect(int id, int wantedDocumentId)
        : base(id)
    {
        WantedDocumentId = wantedDocumentId;
    }

    internal static CharacterReadWantedDocumentEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public WantedDocumentData? GetWantedDocumentData()
    {
        return DofusApi.Datacenter.WantedDocumentsRepository.GetWantedDocumentDataById(WantedDocumentId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, WantedDocumentId);
    }
}
