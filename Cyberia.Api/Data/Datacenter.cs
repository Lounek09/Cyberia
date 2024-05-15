using Cyberia.Api.Managers;
using Cyberia.Langzilla.Enums;

using System.Reflection;
using System.Text.Json;

namespace Cyberia.Api.Data;

public sealed class Datacenter
{
    public Alignments.AlignmentsRepository AlignmentsRepository { get; init; }
    public Audios.AudiosRepository AudiosRepository { get; init; }
    public Breeds.BreedsRepository BreedsRepository { get; init; }
    public Crafts.CraftsRepository CraftsRepository { get; init; }
    public Dialogs.DialogsRepository DialogsRepository { get; init; }
    public Effects.EffectsRepository EffectsRepository { get; init; }
    public Emotes.EmotesRepository EmotesRepository { get; init; }
    public FightChallenges.FightChallengesRepository FightChallengesRepository { get; init; }
    public Guilds.GuildsRepository GuildsRepository { get; init; }
    public Hints.HintsRepository HintsRepository { get; init; }
    public Houses.HousesRepository HousesRepository { get; init; }
    public Incarnations.IncarnationsRepository IncarnationsRepository { get; init; }
    public InteractiveObjects.InteractiveObjectsRepository InteractiveObjectsRepository { get; init; }
    public Items.ItemsRepository ItemsRepository { get; init; }
    public ItemSets.ItemSetsRepository ItemSetsRepository { get; init; }
    public ItemStats.ItemsStatsRepository ItemsStatsRepository { get; init; }
    public Jobs.JobsRepository JobsRepository { get; init; }
    public KnowledgeBook.KnowledgeBookRepository KnowledgeBookRepository { get; init; }
    public Maps.MapsRepository MapsRepository { get; init; }
    public Monsters.MonstersRepository MonstersRepository { get; init; }
    public Npcs.NpcsRepository NpcsRepository { get; init; }
    public Pets.PetsRepository PetsRepository { get; init; }
    public Pvp.PvpRepository PvpRepository { get; init; }
    public Quests.QuestsRepository QuestsRepository { get; init; }
    public Ranks.RanksRepository RanksRepository { get; init; }
    public Rides.RidesRepository RidesRepository { get; init; }
    public Runes.RunesRepository RunesRepository { get; init; }
    public Scripts.ScriptsRepository ScriptsRepository { get; init; }
    public Servers.ServersRepository ServersRepository { get; init; }
    public Skills.SkillsRepository SkillsRepository { get; init; }
    public SpeakingItems.SpeakingItemsRepository SpeakingItemsRepository { get; init; }
    public Spells.SpellsRepository SpellsRepository { get; init; }
    public States.StatesRepository StatesRepository { get; init; }
    public Names.TaxCollectorNamesRepository TaxCollectorNamesRepository { get; init; }
    public TimeZone.TimeZoneRepository TimeZonesRepository { get; init; }
    public Titles.TitlesRepository TitlesRepository { get; init; }
    public TTG.TTGRepository TTGRepository { get; init; }

    internal Datacenter(LangType type)
    {
        var outputDirectoryPath = LangParserManager.GetOutputDirectoryPath(type, LangLanguage.FR);

        AlignmentsRepository = Alignments.AlignmentsRepository.Load(outputDirectoryPath);
        AudiosRepository = Audios.AudiosRepository.Load(outputDirectoryPath);
        BreedsRepository = Breeds.BreedsRepository.Load(outputDirectoryPath);
        CraftsRepository = Crafts.CraftsRepository.Load(outputDirectoryPath);
        DialogsRepository = Dialogs.DialogsRepository.Load(outputDirectoryPath);
        EffectsRepository = Effects.EffectsRepository.Load(outputDirectoryPath);
        EmotesRepository = Emotes.EmotesRepository.Load(outputDirectoryPath);
        FightChallengesRepository = FightChallenges.FightChallengesRepository.Load(outputDirectoryPath);
        GuildsRepository = Guilds.GuildsRepository.Load(outputDirectoryPath);
        HintsRepository = Hints.HintsRepository.Load(outputDirectoryPath);
        HousesRepository = Houses.HousesRepository.Load(outputDirectoryPath);
        IncarnationsRepository = Incarnations.IncarnationsRepository.Load(outputDirectoryPath);
        InteractiveObjectsRepository = InteractiveObjects.InteractiveObjectsRepository.Load(outputDirectoryPath);
        ItemsRepository = Items.ItemsRepository.Load(outputDirectoryPath);
        ItemSetsRepository = ItemSets.ItemSetsRepository.Load(outputDirectoryPath);
        ItemsStatsRepository = ItemStats.ItemsStatsRepository.Load(outputDirectoryPath);
        JobsRepository = Jobs.JobsRepository.Load(outputDirectoryPath);
        KnowledgeBookRepository = KnowledgeBook.KnowledgeBookRepository.Load(outputDirectoryPath);
        MapsRepository = Maps.MapsRepository.Load(outputDirectoryPath);
        MonstersRepository = Monsters.MonstersRepository.Load(outputDirectoryPath);
        NpcsRepository = Npcs.NpcsRepository.Load(outputDirectoryPath);
        PetsRepository = Pets.PetsRepository.Load(outputDirectoryPath);
        PvpRepository = Pvp.PvpRepository.Load(outputDirectoryPath);
        QuestsRepository = Quests.QuestsRepository.Load(outputDirectoryPath);
        RanksRepository = Ranks.RanksRepository.Load(outputDirectoryPath);
        RidesRepository = Rides.RidesRepository.Load(outputDirectoryPath);
        RunesRepository = Runes.RunesRepository.Load(outputDirectoryPath);
        ScriptsRepository = Scripts.ScriptsRepository.Load(outputDirectoryPath);
        ServersRepository = Servers.ServersRepository.Load(outputDirectoryPath);
        SkillsRepository = Skills.SkillsRepository.Load(outputDirectoryPath);
        SpeakingItemsRepository = SpeakingItems.SpeakingItemsRepository.Load(outputDirectoryPath);
        SpellsRepository = Spells.SpellsRepository.Load(outputDirectoryPath);
        StatesRepository = States.StatesRepository.Load(outputDirectoryPath);
        TaxCollectorNamesRepository = Names.TaxCollectorNamesRepository.Load(outputDirectoryPath);
        TimeZonesRepository = TimeZone.TimeZoneRepository.Load(outputDirectoryPath);
        TitlesRepository = Titles.TitlesRepository.Load(outputDirectoryPath);
        TTGRepository = TTG.TTGRepository.Load(outputDirectoryPath);
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

    internal static T LoadRepository<T>(string filePath)
        where T : class, IDofusRepository
    {
        var constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, Type.EmptyTypes) ??
            throw new EntryPointNotFoundException($"Non public parameter-less constructor for {typeof(T).Name} not found");

        if (!File.Exists(filePath))
        {
            Log.Warning("File {FilePath} not found to initialize {TypeName}", filePath, typeof(T).Name);
            return (T)constructor.Invoke(null);
        }

        using var json = File.OpenRead(filePath);

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
