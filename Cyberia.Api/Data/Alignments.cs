using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class AlignmentData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("c")]
        public bool CanConquest { get; init; }

        [JsonConstructor]
        internal AlignmentData()
        {
            Name = string.Empty;
        }

        public bool CanJoin(int alignmentId)
        {
            AlignmentJoinData? alignmentJoinData = DofusApi.Datacenter.AlignmentsData.GetAlignmentJoinDataById(Id);

            return alignmentJoinData is not null && alignmentJoinData.CanJoin(alignmentId);
        }

        public bool CanAttack(int alignmentId)
        {
            AlignmentAttackData? alignmentAttackData = DofusApi.Datacenter.AlignmentsData.GetAlignmentAttackDataById(Id);

            return alignmentAttackData is not null && alignmentAttackData.CanAttack(alignmentId);
        }

        public bool CanViewPvpGain(int alignmentId)
        {
            AlignmentViewPvpGainData? alignmentViewPvpGainData = DofusApi.Datacenter.AlignmentsData.GetAlignmentViewPvpGainDataById(Id);

            return alignmentViewPvpGainData is not null && alignmentViewPvpGainData.CanViewPvpGain(alignmentId);
        }
    }

    internal sealed class AlignmentJoinData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<bool>))]
        public ReadOnlyCollection<bool> Values { get; init; }

        [JsonConstructor]
        internal AlignmentJoinData()
        {
            Values = ReadOnlyCollection<bool>.Empty;
        }

        public bool CanJoin(int targetAlignmentId)
        {
            return Values.Count >= targetAlignmentId && Values[targetAlignmentId];
        }
    }

    internal sealed class AlignmentAttackData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<bool>))]
        public ReadOnlyCollection<bool> Values { get; init; }

        [JsonConstructor]
        internal AlignmentAttackData()
        {
            Values = ReadOnlyCollection<bool>.Empty;
        }

        public bool CanAttack(int targetAlignmentId)
        {
            return Values.Count >= targetAlignmentId && Values[targetAlignmentId];
        }
    }

    public sealed class AlignmentOrderData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("a")]
        public int AlignmentId { get; init; }

        [JsonConstructor]
        internal AlignmentOrderData()
        {
            Name = string.Empty;
        }

        public AlignmentData? GetAlignementData()
        {
            return DofusApi.Datacenter.AlignmentsData.GetAlignmentDataById(AlignmentId);
        }
    }

    internal sealed class AlignmentViewPvpGainData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<bool>))]
        public ReadOnlyCollection<bool> Values { get; init; }

        [JsonConstructor]
        internal AlignmentViewPvpGainData()
        {
            Values = ReadOnlyCollection<bool>.Empty;
        }

        public bool CanViewPvpGain(int targetAlignmentId)
        {
            return Values.Count >= targetAlignmentId && Values[targetAlignmentId];
        }
    }

    public sealed class AlignmentFeatData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("g")]
        public int GfxId { get; init; }

        [JsonPropertyName("e")]
        public int AlignmentFeatEffectId { get; init; }

        [JsonConstructor]
        internal AlignmentFeatData()
        {
            Name = string.Empty;
        }

        public AlignmentFeatEffectData? GetAlignmentFeatEffectData()
        {
            return DofusApi.Datacenter.AlignmentsData.GetAlignmentFeatEffectDataById(AlignmentFeatEffectId);
        }
    }

    public sealed class AlignmentFeatEffectData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Description { get; init; }

        [JsonConstructor]
        internal AlignmentFeatEffectData()
        {
            Description = string.Empty;
        }
    }

    public sealed class AlignmentBalanceData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("s")]
        public int Start { get; init; }

        [JsonPropertyName("e")]
        public int End { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonConstructor]
        internal AlignmentBalanceData()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }

    public sealed class AlignmentSpecializationData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonPropertyName("o")]
        public int AlignmentOrderId { get; init; }

        [JsonPropertyName("av")]
        public int AlignmentLevelRequired { get; init; }

        [JsonPropertyName("f")]
        [JsonInclude]
        //TODO: JsonConverter for AlignmentFeatsParameters in AlignmentSpecializationData
        private List<List<object>> AlignmentFeatsParameters { get; init; }

        [JsonConstructor]
        internal AlignmentSpecializationData()
        {
            Name = string.Empty;
            Description = string.Empty;
            AlignmentFeatsParameters = [];
        }

        public AlignmentOrderData? GetAlignementOrderData()
        {
            return DofusApi.Datacenter.AlignmentsData.GetAlignmentOrderDataById(AlignmentOrderId);
        }
    }

    public sealed class AlignmentsData : IDofusData
    {
        private const string FILE_NAME = "alignment.json";

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
        internal AlignmentsData()
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

        internal static AlignmentsData Load()
        {
            return Datacenter.LoadDataFromFile<AlignmentsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public AlignmentData? GetAlignmentDataById(int id)
        {
            Alignments.TryGetValue(id, out AlignmentData? alignmentData);
            return alignmentData;
        }

        public string GetAlignmentNameById(int id)
        {
            AlignmentData? alignmentData = GetAlignmentDataById(id);

            return alignmentData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : alignmentData.Name;
        }

        internal AlignmentJoinData? GetAlignmentJoinDataById(int id)
        {
            AlignmentsJoin.TryGetValue(id, out AlignmentJoinData? alignmentJoinData);
            return alignmentJoinData;
        }

        internal AlignmentAttackData? GetAlignmentAttackDataById(int id)
        {
            AlignmentsAttack.TryGetValue(id, out AlignmentAttackData? alignmentAttackData);
            return alignmentAttackData;
        }

        public AlignmentOrderData? GetAlignmentOrderDataById(int id)
        {
            AlignmentOrders.TryGetValue(id, out AlignmentOrderData? alignmentOrderData);
            return alignmentOrderData;
        }

        internal AlignmentViewPvpGainData? GetAlignmentViewPvpGainDataById(int id)
        {
            AlignmentsViewPvpGain.TryGetValue(id, out AlignmentViewPvpGainData? alignmentViewPvpGainData);
            return alignmentViewPvpGainData;
        }

        public AlignmentFeatData? GetAlignmentFeatDataById(int id)
        {
            AlignmentFeats.TryGetValue(id, out AlignmentFeatData? alignmentFeatData);
            return alignmentFeatData;
        }

        public string GetAlignmentFeatNameById(int id)
        {
            AlignmentFeatData? alignmentFeatData = GetAlignmentFeatDataById(id);

            return alignmentFeatData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : alignmentFeatData.Name;
        }

        public AlignmentFeatEffectData? GetAlignmentFeatEffectDataById(int id)
        {
            AlignmentFeatEffects.TryGetValue(id, out AlignmentFeatEffectData? alignmentFeatEffectData);
            return alignmentFeatEffectData;
        }

        public AlignmentSpecializationData? GetAlignmentSpecializationDataById(int id)
        {
            AlignmentSpecializations.TryGetValue(id, out AlignmentSpecializationData? alignmentSpecializationData);
            return alignmentSpecializationData;
        }

        public string GetAlignmentSpecializationNameById(int id)
        {
            AlignmentSpecializationData? alignmentSpecializationData = GetAlignmentSpecializationDataById(id);

            return alignmentSpecializationData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : alignmentSpecializationData.Name;
        }
    }
}
