using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.KnowledgeBook.Localized;

internal sealed class KnowledgeBookLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => KnowledgeBookRepository.FileName;

    [JsonPropertyName("KBC")]
    public IReadOnlyList<KnowledgeBookCatagoryLocalizedData> KnowledgeBookCatagories { get; init; }

    [JsonPropertyName("KBA")]
    public IReadOnlyList<KnowledgeBookArticleLocalizedData> KnowledgeBookArticles { get; init; }

    [JsonPropertyName("KBT")]
    public IReadOnlyList<KnowledgeBookTipLocalizedData> KnowledgeBookTips { get; init; }

    [JsonConstructor]
    internal KnowledgeBookLocalizedRepository()
    {
        KnowledgeBookCatagories = ReadOnlyCollection<KnowledgeBookCatagoryLocalizedData>.Empty;
        KnowledgeBookArticles = ReadOnlyCollection<KnowledgeBookArticleLocalizedData>.Empty;
        KnowledgeBookTips = ReadOnlyCollection<KnowledgeBookTipLocalizedData>.Empty;
    }
}
