using Cyberia.Api.Data.Emotes;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Jobs;
using Cyberia.Api.Data.Spells;

using System.Collections.ObjectModel;

namespace Cyberia.Api.Data.Quests;

public sealed class QuestStepRewardsData : IDofusData
{
    public int Experience { get; init; }

    public int Kamas { get; init; }

    public IReadOnlyDictionary<int, int> ItemsIdQuantities { get; init; }

    public IReadOnlyCollection<int> EmotesId { get; init; }

    public IReadOnlyCollection<int> JobsId { get; init; }

    public IReadOnlyCollection<int> SpellsId { get; init; }

    internal QuestStepRewardsData()
    {
        ItemsIdQuantities = ReadOnlyDictionary<int, int>.Empty;
        EmotesId = [];
        JobsId = [];
        SpellsId = [];
    }

    public IEnumerable<ItemData> GetItemsData()
    {
        foreach (var pair in ItemsIdQuantities)
        {
            var itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(pair.Key);
            if (itemData is not null)
            {
                yield return itemData;
            }
        }
    }

    public IReadOnlyDictionary<ItemData, int> GetItemsDataQuantities()
    {
        Dictionary<ItemData, int> itemsDataQuantities = [];

        foreach (var pair in ItemsIdQuantities)
        {
            var itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(pair.Key);
            if (itemData is not null)
            {
                itemsDataQuantities.Add(itemData, pair.Value);
            }
        }

        return itemsDataQuantities;
    }

    public IEnumerable<EmoteData> GetEmotesData()
    {
        foreach (var emoteId in EmotesId)
        {
            var emoteData = DofusApi.Datacenter.EmotesData.GetEmoteById(emoteId);
            if (emoteData is not null)
            {
                yield return emoteData;
            }
        }
    }

    public IEnumerable<JobData> GetJobsData()
    {
        foreach (var jobId in JobsId)
        {
            var jobData = DofusApi.Datacenter.JobsData.GetJobDataById(jobId);
            if (jobData is not null)
            {
                yield return jobData;
            }
        }
    }

    public IEnumerable<SpellData> GetSpellsData()
    {
        foreach (var spellId in SpellsId)
        {
            var spellData = DofusApi.Datacenter.SpellsData.GetSpellDataById(spellId);
            if (spellData is not null)
            {
                yield return spellData;
            }
        }
    }
}
