using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Alignment
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("c")]
        public bool CanConquest { get; init; }

        public Alignment()
        {
            Name = string.Empty;
        }

        public bool CanJoin(int alignmentId)
        {
            AlignmentJoin? alignmentJoin = DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentJoinById(Id);

            return alignmentJoin is not null && alignmentJoin.CanJoin(alignmentId);
        }

        public bool CanAttack(int alignmentId)
        {
            AlignmentAttack? alignmentAttack = DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentAttackById(Id);

            return alignmentAttack is not null && alignmentAttack.CanAttack(alignmentId);
        }

        public bool CanViewPvpGain(int alignmentId)
        {
            AlignmentViewPvpGain? alignmentViewPvpGain = DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentViewPvpGainById(Id);

            return alignmentViewPvpGain is not null && alignmentViewPvpGain.CanViewPvpGain(alignmentId);
        }
    }

    public sealed class AlignmentJoin
    {
        [JsonPropertyName("id")]
        public int AlignmentId { get; init; }

        [JsonPropertyName("v")]
        public List<bool> Values { get; init; }

        public AlignmentJoin()
        {
            Values = new();
        }

        public bool CanJoin(int targetAlignmentId)
        {
            return Values.Count >= targetAlignmentId && Values[targetAlignmentId];
        }
    }

    public sealed class AlignmentAttack
    {
        [JsonPropertyName("id")]
        public int AlignmentId { get; init; }

        [JsonPropertyName("v")]
        public List<bool> Values { get; init; }

        public AlignmentAttack()
        {
            Values = new();
        }

        public bool CanAttack(int targetAlignmentId)
        {
            return Values.Count >= targetAlignmentId && Values[targetAlignmentId];
        }
    }

    public sealed class AlignmentOrder
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("a")]
        public int AlignmentId { get; init; }

        public AlignmentOrder()
        {
            Name = string.Empty;
        }

        public Alignment? GetAlignement()
        {
            return DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentById(AlignmentId);
        }
    }

    public sealed class AlignmentViewPvpGain
    {
        [JsonPropertyName("id")]
        public int AlignmentId { get; init; }

        [JsonPropertyName("v")]
        public List<bool> Values { get; init; }

        public AlignmentViewPvpGain()
        {
            Values = new();
        }

        public bool CanViewPvpGain(int targetAlignmentId)
        {
            return Values.Count >= targetAlignmentId && Values[targetAlignmentId];
        }
    }

    public sealed class AlignmentFeat
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("g")]
        public int GfxId { get; init; }

        [JsonPropertyName("e")]
        public int AlignmentFeatEffectId { get; init; }

        public AlignmentFeat()
        {
            Name = string.Empty;
        }

        public AlignmentFeatEffect? GetAlignmentFeatEffect()
        {
            return DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentFeatEffectById(AlignmentFeatEffectId);
        }
    }

    public sealed class AlignmentFeatEffect
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Description { get; init; }

        public AlignmentFeatEffect()
        {
            Description = string.Empty;
        }
    }

    public sealed class AlignmentBalance
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

        public AlignmentBalance()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }

    public sealed class AlignmentSpecialization
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

        public AlignmentSpecialization()
        {
            Name = string.Empty;
            Description = string.Empty;
            AlignmentFeatsParameters = new();
        }

        public AlignmentOrder? GetAlignementOrder()
        {
            return DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentOrderById(AlignmentOrderId);
        }
    }

    public sealed class AlignmentData
    {
        private const string FILE_NAME = "alignment.json";

        [JsonPropertyName("A.a")]
        public List<Alignment> Alignments { get; init; }

        [JsonPropertyName("A.jo")]
        public List<AlignmentJoin> AlignmentsJoin { get; init; }

        [JsonPropertyName("A.at")]
        public List<AlignmentAttack> AlignmentsAttack { get; init; }

        [JsonPropertyName("A.o")]
        public List<AlignmentOrder> AlignmentOrders { get; init; }

        [JsonPropertyName("A.g")]
        public List<AlignmentViewPvpGain> AlignmentsViewPvpGain { get; init; }

        [JsonPropertyName("A.f")]
        public List<AlignmentFeat> AlignmentFeats { get; init; }

        [JsonPropertyName("A.fe")]
        public List<AlignmentFeatEffect> AlignmentFeatEffects { get; init; }

        [JsonPropertyName("A.b")]
        public List<AlignmentBalance> AlignmentBalance { get; init; }

        [JsonPropertyName("A.s")]
        public List<AlignmentSpecialization> AlignmentSpecializations { get; init; }

        public AlignmentData()
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

        internal static AlignmentData Build()
        {
            return Json.LoadFromFile<AlignmentData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public Alignment? GetAlignmentById(int id)
        {
            return Alignments.Find(x => x.Id == id);
        }

        public string GetAlignmentNameById(int id)
        {
            Alignment? alignment = GetAlignmentById(id);

            return alignment is null ? $"Inconnu ({id})" : alignment.Name;
        }

        public AlignmentJoin? GetAlignmentJoinById(int id)
        {
            return AlignmentsJoin.Find(x => x.AlignmentId == id);
        }

        public AlignmentAttack? GetAlignmentAttackById(int id)
        {
            return AlignmentsAttack.Find(x => x.AlignmentId == id);
        }

        public AlignmentOrder? GetAlignmentOrderById(int id)
        {
            return AlignmentOrders.Find(x => x.Id == id);
        }

        public AlignmentViewPvpGain? GetAlignmentViewPvpGainById(int id)
        {
            return AlignmentsViewPvpGain.Find(x => x.AlignmentId == id);
        }

        public AlignmentFeat? GetAlignmentFeatById(int id)
        {
            return AlignmentFeats.Find(x => x.Id == id);
        }

        public string GetAlignmentFeatNameById(int id)
        {
            AlignmentFeat? alignmentFeat = GetAlignmentFeatById(id);

            return alignmentFeat is null ? $"Inconnu ({id})" : alignmentFeat.Name;
        }

        public AlignmentFeatEffect? GetAlignmentFeatEffectById(int id)
        {
            return AlignmentFeatEffects.Find(x => x.Id == id);
        }

        public AlignmentSpecialization? GetAlignmentSpecializationById(int id)
        {
            return AlignmentSpecializations.Find(x => x.Id == id);
        }

        public string GetAlignmentSpecializationNameById(int id)
        {
            AlignmentSpecialization? alignmentSpecialization = GetAlignmentSpecializationById(id);

            return alignmentSpecialization is null ? $"Inconnu ({id})" : alignmentSpecialization.Name;
        }
    }
}
