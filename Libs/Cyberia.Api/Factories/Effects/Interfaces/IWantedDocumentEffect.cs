using Cyberia.Api.Data.WantedDocuments;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IWantedDocumentEffect
{
    int WantedDocumentId { get; }

    WantedDocumentData? GetWantedDocumentData();
}
