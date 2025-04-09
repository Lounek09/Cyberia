using Cyberia.Api.Data.Alignments.Localized;
using Cyberia.Api.Factories;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments;

public sealed class AlignmentsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "alignment.json";

    [JsonPropertyName("A.a")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AlignmentData>))]
    public FrozenDictionary<int, AlignmentData> Alignments { get; init; }

    [JsonPropertyName("A.jo")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AlignmentJoinData>))]
    internal FrozenDictionary<int, AlignmentJoinData> AlignmentsJoin { get; init; }

    [JsonPropertyName("A.at")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AlignmentAttackData>))]
    internal FrozenDictionary<int, AlignmentAttackData> AlignmentsAttack { get; init; }

    [JsonPropertyName("A.o")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AlignmentOrderData>))]
    public FrozenDictionary<int, AlignmentOrderData> AlignmentOrders { get; init; }

    [JsonPropertyName("A.g")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AlignmentViewPvpGainData>))]
    internal FrozenDictionary<int, AlignmentViewPvpGainData> AlignmentsViewPvpGain { get; init; }

    [JsonPropertyName("A.f")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AlignmentFeatData>))]
    public FrozenDictionary<int, AlignmentFeatData> AlignmentFeats { get; init; }

    [JsonPropertyName("A.fe")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AlignmentFeatEffectData>))]
    public FrozenDictionary<int, AlignmentFeatEffectData> AlignmentFeatEffects { get; init; }

    [JsonPropertyName("A.b")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AlignmentBalanceData>))]
    public FrozenDictionary<int, AlignmentBalanceData> AlignmentBalances { get; init; }

    [JsonPropertyName("A.s")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AlignmentSpecializationData>))]
    public FrozenDictionary<int, AlignmentSpecializationData> AlignmentSpecializations { get; init; }

    [JsonConstructor]
    internal AlignmentsRepository()
    {
        Alignments = FrozenDictionary<int, AlignmentData>.Empty;
        AlignmentsJoin = FrozenDictionary<int, AlignmentJoinData>.Empty;
        AlignmentsAttack = FrozenDictionary<int, AlignmentAttackData>.Empty;
        AlignmentOrders = FrozenDictionary<int, AlignmentOrderData>.Empty;
        AlignmentsViewPvpGain = FrozenDictionary<int, AlignmentViewPvpGainData>.Empty;
        AlignmentFeats = FrozenDictionary<int, AlignmentFeatData>.Empty;
        AlignmentFeatEffects = FrozenDictionary<int, AlignmentFeatEffectData>.Empty;
        AlignmentBalances = FrozenDictionary<int, AlignmentBalanceData>.Empty;
        AlignmentSpecializations = FrozenDictionary<int, AlignmentSpecializationData>.Empty;
    }

    public AlignmentData? GetAlignmentDataById(int id)
    {
        Alignments.TryGetValue(id, out var alignmentData);
        return alignmentData;
    }

    public string GetAlignmentNameById(int id, Language language)
    {
        return GetAlignmentNameById(id, language.ToCulture());
    }

    public string GetAlignmentNameById(int id, CultureInfo? culture = null)
    {
        var alignmentData = GetAlignmentDataById(id);

        return alignmentData is null
            ? Translation.UnknownData(id, culture)
            : alignmentData.Name.ToString(culture);
    }

    internal AlignmentJoinData? GetAlignmentJoinDataById(int id)
    {
        AlignmentsJoin.TryGetValue(id, out var alignmentJoinData);
        return alignmentJoinData;
    }

    internal AlignmentAttackData? GetAlignmentAttackDataById(int id)
    {
        AlignmentsAttack.TryGetValue(id, out var alignmentAttackData);
        return alignmentAttackData;
    }

    public AlignmentOrderData? GetAlignmentOrderDataById(int id)
    {
        AlignmentOrders.TryGetValue(id, out var alignmentOrderData);
        return alignmentOrderData;
    }

    internal AlignmentViewPvpGainData? GetAlignmentViewPvpGainDataById(int id)
    {
        AlignmentsViewPvpGain.TryGetValue(id, out var alignmentViewPvpGainData);
        return alignmentViewPvpGainData;
    }

    public AlignmentFeatData? GetAlignmentFeatDataById(int id)
    {
        AlignmentFeats.TryGetValue(id, out var alignmentFeatData);
        return alignmentFeatData;
    }

    public string GetAlignmentFeatNameById(int id, Language language)
    {
        return GetAlignmentFeatNameById(id, language.ToCulture());
    }

    public string GetAlignmentFeatNameById(int id, CultureInfo? culture = null)
    {
        var alignmentFeatData = GetAlignmentFeatDataById(id);

        return alignmentFeatData is null
            ? Translation.UnknownData(id, culture)
            : alignmentFeatData.Name.ToString(culture);
    }

    public AlignmentFeatEffectData? GetAlignmentFeatEffectDataById(int id)
    {
        AlignmentFeatEffects.TryGetValue(id, out var alignmentFeatEffectData);
        return alignmentFeatEffectData;
    }

    public AlignmentBalanceData? GetAlignmentBalanceDataById(int id)
    {
        AlignmentBalances.TryGetValue(id, out var alignmentBalanceData);
        return alignmentBalanceData;
    }

    public AlignmentSpecializationData? GetAlignmentSpecializationDataById(int id)
    {
        AlignmentSpecializations.TryGetValue(id, out var alignmentSpecializationData);
        return alignmentSpecializationData;
    }

    public string GetAlignmentSpecializationNameById(int id, Language language)
    {
        return GetAlignmentSpecializationNameById(id, language.ToCulture());
    }

    public string GetAlignmentSpecializationNameById(int id, CultureInfo? culture = null)
    {
        var alignmentSpecializationData = GetAlignmentSpecializationDataById(id);

        return alignmentSpecializationData is null
            ? Translation.UnknownData(id, culture)
            : alignmentSpecializationData.Name.ToString(culture);
    }

    protected override void LoadLocalizedData(LangType type, Language language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<AlignmentsLocalizedRepository>(type, language);

        foreach (var alignmentLocalizedData in localizedRepository.Alignments)
        {
            var alignmentData = GetAlignmentDataById(alignmentLocalizedData.Id);
            alignmentData?.Name.TryAdd(twoLetterISOLanguageName, alignmentLocalizedData.Name);
        }

        foreach (var alignmentOrderLocalizedData in localizedRepository.AlignmentOrders)
        {
            var alignmentOrderData = GetAlignmentOrderDataById(alignmentOrderLocalizedData.Id);
            alignmentOrderData?.Name.TryAdd(twoLetterISOLanguageName, alignmentOrderLocalizedData.Name);
        }

        foreach (var alignmentFeatLocalizedData in localizedRepository.AlignmentFeats)
        {
            var alignmentFeatData = GetAlignmentFeatDataById(alignmentFeatLocalizedData.Id);
            alignmentFeatData?.Name.TryAdd(twoLetterISOLanguageName, alignmentFeatLocalizedData.Name);
        }

        foreach (var alignmentFeatEffectLocalizedData in localizedRepository.AlignmentFeatEffects)
        {
            var alignmentFeatEffectData = GetAlignmentFeatEffectDataById(alignmentFeatEffectLocalizedData.Id);
            alignmentFeatEffectData?.Description.TryAdd(twoLetterISOLanguageName, alignmentFeatEffectLocalizedData.Description);
        }

        foreach (var alignmentBalanceLocalizedData in localizedRepository.AlignmentBalances)
        {
            var alignmentBalanceData = GetAlignmentBalanceDataById(alignmentBalanceLocalizedData.Id);
            alignmentBalanceData?.Name.TryAdd(twoLetterISOLanguageName, alignmentBalanceLocalizedData.Name);
        }

        foreach (var alignmentSpecializationLocalizedData in localizedRepository.AlignmentSpecializations)
        {
            var alignmentSpecializationData = GetAlignmentSpecializationDataById(alignmentSpecializationLocalizedData.Id);
            if (alignmentSpecializationData is not null)
            {
                alignmentSpecializationData?.Name.TryAdd(twoLetterISOLanguageName, alignmentSpecializationLocalizedData.Name);
                alignmentSpecializationData?.Description.TryAdd(twoLetterISOLanguageName, alignmentSpecializationLocalizedData.Description);

            }
        }
    }

    protected override void FinalizeLoading()
    {
        foreach (var alignmentSpecializationData in AlignmentSpecializations)
        {
            List<AlignmentFeatParametersData> alignmentFeatsParametersData = [];

            foreach (var compressedAlignmentFeatsParameters in alignmentSpecializationData.Value.CompressedAlignmentFeatsParameters)
            {
                var length = compressedAlignmentFeatsParameters.GetArrayLength();

                var alignmentFeatId = compressedAlignmentFeatsParameters[0].GetInt32OrDefault();
                var level = compressedAlignmentFeatsParameters[1].GetInt32OrDefault();
                var parameters = length > 2
                ? JsonSerializer.Deserialize<int[]>(compressedAlignmentFeatsParameters[2]) ?? []
                    : [];

                var alignmentFeatData = GetAlignmentFeatDataById(alignmentFeatId);
                var alignmentFeatEffect = alignmentFeatData is null || alignmentFeatData.AlignmentFeatEffectId == 0
                    ? null
                    : AlignmentFeatEffectFactory.Create(alignmentFeatData.AlignmentFeatEffectId, parameters);

                alignmentFeatsParametersData.Add(new AlignmentFeatParametersData
                {
                    AlignmentFeatId = alignmentFeatId,
                    Level = level,
                    AlignmentFeatEffect = alignmentFeatEffect
                });
            }

            alignmentSpecializationData.Value.AlignmentFeatsParametersData = alignmentFeatsParametersData;
        }
    }
}
