﻿using Cyberia.Api.Factories;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments;

public sealed class AlignmentsRepository : IDofusRepository
{
    private const string c_fileName = "alignment.json";

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
    public FrozenDictionary<int, AlignmentBalanceData> AlignmentBalance { get; init; }

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
        AlignmentBalance = FrozenDictionary<int, AlignmentBalanceData>.Empty;
        AlignmentSpecializations = FrozenDictionary<int, AlignmentSpecializationData>.Empty;
    }

    internal static AlignmentsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        var data = Datacenter.LoadRepository<AlignmentsRepository>(filePath);

        foreach (var alignmentSpecializationData in data.AlignmentSpecializations)
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

                var alignmentFeatData = data.GetAlignmentFeatDataById(alignmentFeatId);
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

        return data;
    }

    public AlignmentData? GetAlignmentDataById(int id)
    {
        Alignments.TryGetValue(id, out var alignmentData);
        return alignmentData;
    }

    public string GetAlignmentNameById(int id)
    {
        var alignmentData = GetAlignmentDataById(id);

        return alignmentData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : alignmentData.Name;
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

    public string GetAlignmentFeatNameById(int id)
    {
        var alignmentFeatData = GetAlignmentFeatDataById(id);

        return alignmentFeatData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : alignmentFeatData.Name;
    }

    public AlignmentFeatEffectData? GetAlignmentFeatEffectDataById(int id)
    {
        AlignmentFeatEffects.TryGetValue(id, out var alignmentFeatEffectData);
        return alignmentFeatEffectData;
    }

    public AlignmentSpecializationData? GetAlignmentSpecializationDataById(int id)
    {
        AlignmentSpecializations.TryGetValue(id, out var alignmentSpecializationData);
        return alignmentSpecializationData;
    }

    public string GetAlignmentSpecializationNameById(int id)
    {
        var alignmentSpecializationData = GetAlignmentSpecializationDataById(id);

        return alignmentSpecializationData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : alignmentSpecializationData.Name;
    }
}