using Cyberia.Api.Data.Alignments;
using Cyberia.Api.Managers;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters;

public sealed class MonsterData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("g")]
    public int GfxId { get; init; }

    [JsonPropertyName("b")]
    public int MonsterRaceId { get; init; }

    [JsonPropertyName("a")]
    public int AlignmentId { get; init; }

    [JsonPropertyName("k")]
    public bool Kickable { get; init; }

    [JsonPropertyName("d")]
    public bool Boss { get; init; }

    [JsonPropertyName("s")]
    public bool SearcheableInBigStore { get; init; }

    [JsonPropertyName("g1")]
    public MonsterGradeData? MonsterGradeData1 { get; init; }

    [JsonPropertyName("g2")]
    public MonsterGradeData? MonsterGradeData2 { get; init; }

    [JsonPropertyName("g3")]
    public MonsterGradeData? MonsterGradeData3 { get; init; }

    [JsonPropertyName("g4")]
    public MonsterGradeData? MonsterGradeData4 { get; init; }

    [JsonPropertyName("g5")]
    public MonsterGradeData? MonsterGradeData5 { get; init; }

    [JsonPropertyName("g6")]
    public MonsterGradeData? MonsterGradeData6 { get; init; }

    [JsonPropertyName("g7")]
    public MonsterGradeData? MonsterGradeData7 { get; init; }

    [JsonPropertyName("g8")]
    public MonsterGradeData? MonsterGradeData8 { get; init; }

    [JsonPropertyName("g9")]
    public MonsterGradeData? MonsterGradeData9 { get; init; }

    [JsonPropertyName("g10")]
    public MonsterGradeData? MonsterGradeData10 { get; init; }

    [JsonIgnore]
    public bool BreedSummon { get; internal set; }

    [JsonIgnore]
    public string TrelloUrl { get; internal set; }

    [JsonConstructor]
    internal MonsterData()
    {
        Name = string.Empty;
        TrelloUrl = string.Empty;
    }

    public async Task<string> GetBigImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("artworks/big", GfxId, size);
    }

    public async Task<string> GetFaceImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("artworks/faces", GfxId, size);
    }

    public async Task<string> GetMiniImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("artworks/mini", GfxId, size);
    }

    public MonsterRaceData? GetMonsterRaceData()
    {
        return DofusApi.Datacenter.MonstersData.GetMonsterRaceDataById(MonsterRaceId);
    }

    public AlignmentData? GetAlignmentData()
    {
        return DofusApi.Datacenter.AlignmentsData.GetAlignmentDataById(AlignmentId);
    }

    public MonsterGradeData? GetMonsterGradeData(int grade = 1)
    {
        return grade switch
        {
            1 => MonsterGradeData1,
            2 => MonsterGradeData2,
            3 => MonsterGradeData3,
            4 => MonsterGradeData4,
            5 => MonsterGradeData5,
            6 => MonsterGradeData6,
            7 => MonsterGradeData7,
            8 => MonsterGradeData8,
            9 => MonsterGradeData9,
            10 => MonsterGradeData10,
            _ => null,
        };
    }

    public IEnumerable<MonsterGradeData> GetMonsterGradesData()
    {
        for (var i = 1; i <= 10; i++)
        {
            var monsterGradeData = GetMonsterGradeData(i);
            if (monsterGradeData is not null)
            {
                yield return monsterGradeData;
            }
        }
    }

    public int GetMaxGradeNumber()
    {
        for (var i = 10; i > 0; i--)
        {
            if (GetMonsterGradeData(i) is not null)
            {
                return i;
            }
        }

        return -1;
    }

    public int GetMinLevel()
    {
        return MonsterGradeData1 is null ? -1 : MonsterGradeData1.Level;
    }

    public int GetMaxLevel()
    {
        var monsterGradeData = GetMonsterGradeData(GetMaxGradeNumber());
        return monsterGradeData is null ? -1 : monsterGradeData.Level;
    }
}
