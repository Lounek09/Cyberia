using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data;

public sealed class SkillData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonPropertyName("j")]
    public int JobId { get; init; }

    [JsonPropertyName("io")]
    public int InteractiveObjectId { get; init; }

    [JsonPropertyName("c")]
    public string Criterion { get; init; }

    [JsonPropertyName("f")]
    public int ItemTypeIdForgemagus { get; init; }

    [JsonPropertyName("cl")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
    public ReadOnlyCollection<int> CraftsId { get; init; }

    [JsonPropertyName("i")]
    public int HarvestedItemId { get; init; }

    [JsonConstructor]
    internal SkillData()
    {
        Description = string.Empty;
        Criterion = string.Empty;
        CraftsId = ReadOnlyCollection<int>.Empty;
    }

    public JobData? GetJobData()
    {
        return DofusApi.Datacenter.JobsData.GetJobDataById(JobId);
    }

    public InteractiveObjectData? GetInteractiveObjectData()
    {
        return DofusApi.Datacenter.InteractiveObjectsData.GetInteractiveObjectDataById(InteractiveObjectId);
    }

    public ItemTypeData? GetItemTypeDataForgemagus()
    {
        return DofusApi.Datacenter.ItemsData.GetItemTypeDataById(ItemTypeIdForgemagus);
    }

    public IEnumerable<CraftData> GetCraftsData()
    {
        foreach (var craftId in CraftsId)
        {
            var craftData = DofusApi.Datacenter.CraftsData.GetCraftDataById(craftId);
            if (craftData is not null)
            {
                yield return craftData;
            }
        }
    }

    public ItemData? GetHarvestedItemData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemDataById(HarvestedItemId);
    }
}

public sealed class SkillsData : IDofusData
{
    private const string FILE_NAME = "skills.json";

    [JsonPropertyName("SK")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, SkillData>))]
    public FrozenDictionary<int, SkillData> Skills { get; init; }

    [JsonConstructor]
    internal SkillsData()
    {
        Skills = FrozenDictionary<int, SkillData>.Empty;
    }

    internal static SkillsData Load()
    {
        return Datacenter.LoadDataFromFile<SkillsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public SkillData? GetSkillDataById(int id)
    {
        Skills.TryGetValue(id, out var skillData);
        return skillData;
    }
}
