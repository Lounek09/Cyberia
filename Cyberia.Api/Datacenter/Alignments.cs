using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class AlignmentData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("c")]
        public bool CanConquest { get; init; }

        public AlignmentData()
        {
            Name = string.Empty;
        }

        public bool CanJoin(int alignmentId)
        {
            AlignmentJoinData? alignmentJoinData = DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentJoinDataById(Id);

            return alignmentJoinData is not null && alignmentJoinData.CanJoin(alignmentId);
        }

        public bool CanAttack(int alignmentId)
        {
            AlignmentAttackData? alignmentAttackData = DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentAttackDataById(Id);

            return alignmentAttackData is not null && alignmentAttackData.CanAttack(alignmentId);
        }

        public bool CanViewPvpGain(int alignmentId)
        {
            AlignmentViewPvpGainData? alignmentViewPvpGainData = DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentViewPvpGainDataById(Id);

            return alignmentViewPvpGainData is not null && alignmentViewPvpGainData.CanViewPvpGain(alignmentId);
        }
    }

    public sealed class AlignmentJoinData
    {
        [JsonPropertyName("id")]
        public int AlignmentId { get; init; }

        [JsonPropertyName("v")]
        public List<bool> Values { get; init; }

        public AlignmentJoinData()
        {
            Values = new();
        }

        public bool CanJoin(int targetAlignmentId)
        {
            return Values.Count >= targetAlignmentId && Values[targetAlignmentId];
        }
    }

    public sealed class AlignmentAttackData
    {
        [JsonPropertyName("id")]
        public int AlignmentId { get; init; }

        [JsonPropertyName("v")]
        public List<bool> Values { get; init; }

        public AlignmentAttackData()
        {
            Values = new();
        }

        public bool CanAttack(int targetAlignmentId)
        {
            return Values.Count >= targetAlignmentId && Values[targetAlignmentId];
        }
    }

    public sealed class AlignmentOrderData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("a")]
        public int AlignmentId { get; init; }

        public AlignmentOrderData()
        {
            Name = string.Empty;
        }

        public AlignmentData? GetAlignementData()
        {
            return DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentDataById(AlignmentId);
        }
    }

    public sealed class AlignmentViewPvpGainData
    {
        [JsonPropertyName("id")]
        public int AlignmentId { get; init; }

        [JsonPropertyName("v")]
        public List<bool> Values { get; init; }

        public AlignmentViewPvpGainData()
        {
            Values = new();
        }

        public bool CanViewPvpGain(int targetAlignmentId)
        {
            return Values.Count >= targetAlignmentId && Values[targetAlignmentId];
        }
    }

    public sealed class AlignmentFeatData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("g")]
        public int GfxId { get; init; }

        [JsonPropertyName("e")]
        public int AlignmentFeatEffectId { get; init; }

        public AlignmentFeatData()
        {
            Name = string.Empty;
        }

        public AlignmentFeatEffectData? GetAlignmentFeatEffectData()
        {
            return DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentFeatEffectDataById(AlignmentFeatEffectId);
        }
    }

    public sealed class AlignmentFeatEffectData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Description { get; init; }

        public AlignmentFeatEffectData()
        {
            Description = string.Empty;
        }
    }

    public sealed class AlignmentBalanceData
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

        public AlignmentBalanceData()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }

    public sealed class AlignmentSpecializationData
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
        //TODO: jsonconverter for AlignmentFeatsParameters in AlignmentSpecialization
        public List<List<object>> AlignmentFeatsParameters { get; init; }

        public AlignmentSpecializationData()
        {
            Name = string.Empty;
            Description = string.Empty;
            AlignmentFeatsParameters = new();
        }

        public AlignmentOrderData? GetAlignementOrderData()
        {
            return DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentOrderDataById(AlignmentOrderId);
        }
    }

    public sealed class AlignmentsData
    {
        private const string FILE_NAME = "alignment.json";

        [JsonPropertyName("A.a")]
        public List<AlignmentData> Alignments { get; init; }

        [JsonPropertyName("A.jo")]
        public List<AlignmentJoinData> AlignmentsJoin { get; init; }

        [JsonPropertyName("A.at")]
        public List<AlignmentAttackData> AlignmentsAttack { get; init; }

        [JsonPropertyName("A.o")]
        public List<AlignmentOrderData> AlignmentOrders { get; init; }

        [JsonPropertyName("A.g")]
        public List<AlignmentViewPvpGainData> AlignmentsViewPvpGain { get; init; }

        [JsonPropertyName("A.f")]
        public List<AlignmentFeatData> AlignmentFeats { get; init; }

        [JsonPropertyName("A.fe")]
        public List<AlignmentFeatEffectData> AlignmentFeatEffects { get; init; }

        [JsonPropertyName("A.b")]
        public List<AlignmentBalanceData> AlignmentBalance { get; init; }

        [JsonPropertyName("A.s")]
        public List<AlignmentSpecializationData> AlignmentSpecializations { get; init; }

        public AlignmentsData()
        {
            Alignments = new();
            AlignmentsJoin = new();
            AlignmentsAttack = new();
            AlignmentOrders = new();
            AlignmentsViewPvpGain = new();
            AlignmentFeats = new();
            AlignmentFeatEffects = new();
            AlignmentBalance = new();
            AlignmentSpecializations = new();
        }

        internal static AlignmentsData Build()
        {
            return Json.LoadFromFile<AlignmentsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public AlignmentData? GetAlignmentDataById(int id)
        {
            return Alignments.Find(x => x.Id == id);
        }

        public string GetAlignmentNameById(int id)
        {
            AlignmentData? alignmentData = GetAlignmentDataById(id);

            return alignmentData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : alignmentData.Name;
        }

        public AlignmentJoinData? GetAlignmentJoinDataById(int id)
        {
            return AlignmentsJoin.Find(x => x.AlignmentId == id);
        }

        public AlignmentAttackData? GetAlignmentAttackDataById(int id)
        {
            return AlignmentsAttack.Find(x => x.AlignmentId == id);
        }

        public AlignmentOrderData? GetAlignmentOrderDataById(int id)
        {
            return AlignmentOrders.Find(x => x.Id == id);
        }

        public AlignmentViewPvpGainData? GetAlignmentViewPvpGainDataById(int id)
        {
            return AlignmentsViewPvpGain.Find(x => x.AlignmentId == id);
        }

        public AlignmentFeatData? GetAlignmentFeatDataById(int id)
        {
            return AlignmentFeats.Find(x => x.Id == id);
        }

        public string GetAlignmentFeatNameById(int id)
        {
            AlignmentFeatData? alignmentFeatData = GetAlignmentFeatDataById(id);

            return alignmentFeatData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : alignmentFeatData.Name;
        }

        public AlignmentFeatEffectData? GetAlignmentFeatEffectDataById(int id)
        {
            return AlignmentFeatEffects.Find(x => x.Id == id);
        }

        public AlignmentSpecializationData? GetAlignmentSpecializationDataById(int id)
        {
            return AlignmentSpecializations.Find(x => x.Id == id);
        }

        public string GetAlignmentSpecializationNameById(int id)
        {
            AlignmentSpecializationData? alignmentSpecializationData = GetAlignmentSpecializationDataById(id);

            return alignmentSpecializationData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : alignmentSpecializationData.Name;
        }
    }
}
