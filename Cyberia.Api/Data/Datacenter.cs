using System.Reflection;
using System.Text.Json;

namespace Cyberia.Api.Data;

public sealed class Datacenter
{
    public Aligments.AlignmentsData AlignmentsData { get; init; }
    public Audios.AudiosData AudiosData { get; init; }
    public Breeds.BreedsData BreedsData { get; init; }
    public Crafts.CraftsData CraftsData { get; init; }
    public Dialogs.DialogsData DialogsData { get; init; }
    public Effects.EffectsData EffectsData { get; init; }
    public Emotes.EmotesData EmotesData { get; init; }
    public FightChallenges.FightChallengesData FightChallengesData { get; init; }
    public Guilds.GuildsData GuildsData { get; init; }
    public Hints.HintsData HintsData { get; init; }
    public Houses.HousesData HousesData { get; init; }
    public Incarnations.IncarnationsData IncarnationsData { get; init; }
    public InteractiveObjects.InteractiveObjectsData InteractiveObjectsData { get; init; }
    public Items.ItemsData ItemsData { get; init; }
    public ItemSets.ItemSetsData ItemSetsData { get; init; }
    public ItemStats.ItemsStatsData ItemsStatsData { get; init; }
    public Jobs.JobsData JobsData { get; init; }
    public KnowledgeBook.KnowledgeBookData KnowledgeBookData { get; init; }
    public Maps.MapsData MapsData { get; init; }
    public Monsters.MonstersData MonstersData { get; init; }
    public Npcs.NpcsData NpcsData { get; init; }
    public Pets.PetsData PetsData { get; init; }
    public Pvp.PvpData PvpData { get; init; }
    public Quests.QuestsData QuestsData { get; init; }
    public Ranks.RanksData RanksData { get; init; }
    public Rides.RidesData RidesData { get; init; }
    public Runes.RunesData RunesData { get; init; }
    public Scripts.ScriptsData ScriptsData { get; init; }
    public Servers.ServersData ServersData { get; init; }
    public Skills.SkillsData SkillsData { get; init; }
    public SpeakingItems.SpeakingItemsData SpeakingItemsData { get; init; }
    public Spells.SpellsData SpellsData { get; init; }
    public States.StatesData StatesData { get; init; }
    public Names.TaxCollectorNamesData TaxCollectorNamesData { get; init; }
    public TimeZone.TimeZoneData TimeZonesData { get; init; }
    public Titles.TitlesData TitlesData { get; init; }
    public TTG.TTGData TTGData { get; init; }

    public Datacenter()
    {
        AlignmentsData = Aligments.AlignmentsData.Load();
        AudiosData = Audios.AudiosData.Load();
        BreedsData = Breeds.BreedsData.Load();
        CraftsData = Crafts.CraftsData.Load();
        DialogsData = Dialogs.DialogsData.Load();
        EffectsData = Effects.EffectsData.Load();
        EmotesData = Emotes.EmotesData.Load();
        FightChallengesData = FightChallenges.FightChallengesData.Load();
        GuildsData = Guilds.GuildsData.Load();
        HintsData = Hints.HintsData.Load();
        HousesData = Houses.HousesData.Load();
        IncarnationsData = Incarnations.IncarnationsData.Load();
        InteractiveObjectsData = InteractiveObjects.InteractiveObjectsData.Load();
        ItemsData = Items.ItemsData.Load();
        ItemSetsData = ItemSets.ItemSetsData.Load();
        ItemsStatsData = ItemStats.ItemsStatsData.Load();
        JobsData = Jobs.JobsData.Load();
        KnowledgeBookData = KnowledgeBook.KnowledgeBookData.Load();
        MapsData = Maps.MapsData.Load();
        MonstersData = Monsters.MonstersData.Load();
        NpcsData = Npcs.NpcsData.Load();
        PetsData = Pets.PetsData.Load();
        PvpData = Pvp.PvpData.Load();
        QuestsData = Quests.QuestsData.Load();
        RanksData = Ranks.RanksData.Load();
        RidesData = Rides.RidesData.Load();
        RunesData = Runes.RunesData.Load();
        ScriptsData = Scripts.ScriptsData.Load();
        ServersData = Servers.ServersData.Load();
        SkillsData = Skills.SkillsData.Load();
        SpeakingItemsData = SpeakingItems.SpeakingItemsData.Load();
        SpellsData = Spells.SpellsData.Load();
        StatesData = States.StatesData.Load();
        TaxCollectorNamesData = Names.TaxCollectorNamesData.Load();
        TimeZonesData = TimeZone.TimeZoneData.Load();
        TitlesData = Titles.TitlesData.Load();
        TTGData = TTG.TTGData.Load();
    }

    internal static T LoadDataFromFile<T>(string filePath)
    {
        var constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, Type.EmptyTypes) ??
            throw new EntryPointNotFoundException($"Non public parameter-less constructor for {typeof(T).Name} not found");

        if (!File.Exists(filePath))
        {
            Log.Warning("File {FilePath} not found to initialize {TypeName}", filePath, typeof(T).Name);
            return (T)constructor.Invoke(null);
        }

        var json = File.ReadAllText(filePath);

        try
        {
            return JsonSerializer.Deserialize<T>(json) ?? (T)constructor.Invoke(null);
        }
        catch (Exception e)
        {
            Log.Error(e, "Failed to deserialize the JSON located at {FilePath} to initialize {TypeName}", filePath, typeof(T).Name);
        }

        return (T)constructor.Invoke(null);
    }
}
