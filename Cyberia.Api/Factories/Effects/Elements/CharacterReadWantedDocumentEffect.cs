using Cyberia.Api.Data.WantedDocuments;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterReadWantedDocumentEffect : Effect
{
    public int WantedDocumentId { get; init; }

    private CharacterReadWantedDocumentEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int wantedDocumentId)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        WantedDocumentId = wantedDocumentId;
    }

    internal static CharacterReadWantedDocumentEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param3);
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
