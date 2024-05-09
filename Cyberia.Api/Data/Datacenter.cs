using System.Reflection;
using System.Text.Json;

namespace Cyberia.Api.Data;

public sealed class Datacenter
{
    public required Alignments.AlignmentsData AlignmentsData { get; init; }
    public required Audios.AudiosData AudiosData { get; init; }
    public required Breeds.BreedsData BreedsData { get; init; }
    public required Crafts.CraftsData CraftsData { get; init; }
    public required Dialogs.DialogsData DialogsData { get; init; }
    public required Effects.EffectsData EffectsData { get; init; }
    public required Emotes.EmotesData EmotesData { get; init; }
    public required FightChallenges.FightChallengesData FightChallengesData { get; init; }
    public required Guilds.GuildsData GuildsData { get; init; }
    public required Hints.HintsData HintsData { get; init; }
    public required Houses.HousesData HousesData { get; init; }
    public required Incarnations.IncarnationsData IncarnationsData { get; init; }
    public required InteractiveObjects.InteractiveObjectsData InteractiveObjectsData { get; init; }
    public required Items.ItemsData ItemsData { get; init; }
    public required ItemSets.ItemSetsData ItemSetsData { get; init; }
    public required ItemStats.ItemsStatsData ItemsStatsData { get; init; }
    public required Jobs.JobsData JobsData { get; init; }
    public required KnowledgeBook.KnowledgeBookData KnowledgeBookData { get; init; }
    public required Maps.MapsData MapsData { get; init; }
    public required Monsters.MonstersData MonstersData { get; init; }
    public required Npcs.NpcsData NpcsData { get; init; }
    public required Pets.PetsData PetsData { get; init; }
    public required Pvp.PvpData PvpData { get; init; }
    public required Quests.QuestsData QuestsData { get; init; }
    public required Ranks.RanksData RanksData { get; init; }
    public required Rides.RidesData RidesData { get; init; }
    public required Runes.RunesData RunesData { get; init; }
    public required Scripts.ScriptsData ScriptsData { get; init; }
    public required Servers.ServersData ServersData { get; init; }
    public required Skills.SkillsData SkillsData { get; init; }
    public required SpeakingItems.SpeakingItemsData SpeakingItemsData { get; init; }
    public required Spells.SpellsData SpellsData { get; init; }
    public required States.StatesData StatesData { get; init; }
    public required Names.TaxCollectorNamesData TaxCollectorNamesData { get; init; }
    public required TimeZone.TimeZoneData TimeZonesData { get; init; }
    public required Titles.TitlesData TitlesData { get; init; }
    public required TTG.TTGData TTGData { get; init; }

    private Datacenter()
    {
        
    }

    public static async Task<Datacenter> LoadAsync()
    {
        var alignmentsDataTask = Alignments.AlignmentsData.LoadAsync();
        var audiosDataTask = Audios.AudiosData.LoadAsync();
        var breedsDataTask = Breeds.BreedsData.LoadAsync();
        var craftsDataTask = Crafts.CraftsData.LoadAsync();
        var dialogsDataTask = Dialogs.DialogsData.LoadAsync();
        var effectsDataTask = Effects.EffectsData.LoadAsync();
        var emotesDataTask = Emotes.EmotesData.LoadAsync();
        var fightChallengesDataTask = FightChallenges.FightChallengesData.LoadAsync();
        var guildsDataTask = Guilds.GuildsData.LoadAsync();
        var hintsDataTask = Hints.HintsData.LoadAsync();
        var housesDataTask = Houses.HousesData.LoadAsync();
        var incarnationsDataTask = Incarnations.IncarnationsData.LoadAsync();
        var interactiveObjectsDataTask = InteractiveObjects.InteractiveObjectsData.LoadAsync();
        var itemsDataTask = Items.ItemsData.LoadAsync();
        var itemSetsDataTask = ItemSets.ItemSetsData.LoadAsync();
        var itemsStatsDataTask = ItemStats.ItemsStatsData.LoadAsync();
        var jobsDataTask = Jobs.JobsData.LoadAsync();
        var knowledgeBookDataTask = KnowledgeBook.KnowledgeBookData.LoadAsync();
        var mapsDataTask = Maps.MapsData.LoadAsync();
        var monstersDataTask = Monsters.MonstersData.LoadAsync();
        var npcsDataTask = Npcs.NpcsData.LoadAsync();
        var petsDataTask = Pets.PetsData.LoadAsync();
        var pvpDataTask = Pvp.PvpData.LoadAsync();
        var questsDataTask = Quests.QuestsData.LoadAsync();
        var ranksDataTask = Ranks.RanksData.LoadAsync();
        var ridesDataTask = Rides.RidesData.LoadAsync();
        var runesDataTask = Runes.RunesData.LoadAsync();
        var scriptsDataTask = Scripts.ScriptsData.LoadAsync();
        var serversDataTask = Servers.ServersData.LoadAsync();
        var skillsDataTask = Skills.SkillsData.LoadAsync();
        var speakingItemsDataTask = SpeakingItems.SpeakingItemsData.LoadAsync();
        var spellsDataTask = Spells.SpellsData.LoadAsync();
        var statesDataTask = States.StatesData.LoadAsync();
        var taxCollectorNamesDataTask = Names.TaxCollectorNamesData.LoadAsync();
        var timeZonesDataTask = TimeZone.TimeZoneData.LoadAsync();
        var titlesDataTask = Titles.TitlesData.LoadAsync();
        var ttgDataTask = TTG.TTGData.LoadAsync();

        return new Datacenter
        {
            AlignmentsData = await alignmentsDataTask,
            AudiosData = await audiosDataTask,
            BreedsData = await breedsDataTask,
            CraftsData = await craftsDataTask,
            DialogsData = await dialogsDataTask,
            EffectsData = await effectsDataTask,
            EmotesData = await emotesDataTask,
            FightChallengesData = await fightChallengesDataTask,
            GuildsData = await guildsDataTask,
            HintsData = await hintsDataTask,
            HousesData = await housesDataTask,
            IncarnationsData = await incarnationsDataTask,
            InteractiveObjectsData = await interactiveObjectsDataTask,
            ItemsData = await itemsDataTask,
            ItemSetsData = await itemSetsDataTask,
            ItemsStatsData = await itemsStatsDataTask,
            JobsData = await jobsDataTask,
            KnowledgeBookData = await knowledgeBookDataTask,
            MapsData = await mapsDataTask,
            MonstersData = await monstersDataTask,
            NpcsData = await npcsDataTask,
            PetsData = await petsDataTask,
            PvpData = await pvpDataTask,
            QuestsData = await questsDataTask,
            RanksData = await ranksDataTask,
            RidesData = await ridesDataTask,
            RunesData = await runesDataTask,
            ScriptsData = await scriptsDataTask,
            ServersData = await serversDataTask,
            SkillsData = await skillsDataTask,
            SpeakingItemsData = await speakingItemsDataTask,
            SpellsData = await spellsDataTask,
            StatesData = await statesDataTask,
            TaxCollectorNamesData = await taxCollectorNamesDataTask,
            TimeZonesData = await timeZonesDataTask,
            TitlesData = await titlesDataTask,
            TTGData = await ttgDataTask
        };
    }

    public static (int boost, int cost) GetNextBoostCost(IReadOnlyList<IReadOnlyList<int>> boostCost, int currentAmout)
    {
        foreach (var element in Enumerable.Reverse(boostCost))
        {
            if (element.Count < 2)
            {
                continue;
            }

            if (currentAmout < element[0])
            {
                return (element[1], element.Count > 2 ? element[3] : 1);
            }
        }

        return (1, 1);
    }

    internal static async Task<T> LoadDataAsync<T>(string filePath)
    {
        var constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, Type.EmptyTypes) ??
            throw new EntryPointNotFoundException($"Non public required parameter-less constructor for {typeof(T).Name} not found");

        if (!File.Exists(filePath))
        {
            Log.Warning("File {FilePath} not found to initialize {TypeName}", filePath, typeof(T).Name);
            return (T)constructor.Invoke(null);
        }

        using var json = File.OpenRead(filePath);

        try
        {
            return await JsonSerializer.DeserializeAsync<T>(json) ?? (T)constructor.Invoke(null);
        }
        catch (Exception e)
        {
            Log.Error(e, "Failed to deserialize the JSON located at {FilePath} to initialize {TypeName}", filePath, typeof(T).Name);
        }

        return (T)constructor.Invoke(null);
    }
}
