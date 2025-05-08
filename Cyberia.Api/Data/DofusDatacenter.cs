using Cyberia.Api.Utils;
using Cyberia.Langzilla.Enums;

namespace Cyberia.Api.Data;

/// <summary>
/// Represents the Dofus datacenter that all the static data loaded from the langs files.
/// </summary>
public sealed class DofusDatacenter
{
    public Alignments.AlignmentsRepository AlignmentsRepository { get; private set; }
    public Audios.AudiosRepository AudiosRepository { get; private set; }
    public Breeds.BreedsRepository BreedsRepository { get; private set; }
    public Crafts.CraftsRepository CraftsRepository { get; private set; }
    public Dialogs.DialogsRepository DialogsRepository { get; private set; }
    public Effects.EffectsRepository EffectsRepository { get; private set; }
    public Emotes.EmotesRepository EmotesRepository { get; private set; }
    public FightChallenges.FightChallengesRepository FightChallengesRepository { get; private set; }
    public Guilds.GuildsRepository GuildsRepository { get; private set; }
    public Hints.HintsRepository HintsRepository { get; private set; }
    public Houses.HousesRepository HousesRepository { get; private set; }
    public Incarnations.IncarnationsRepository IncarnationsRepository { get; private set; }
    public InteractiveObjects.InteractiveObjectsRepository InteractiveObjectsRepository { get; private set; }
    public Items.ItemsRepository ItemsRepository { get; private set; }
    public ItemSets.ItemSetsRepository ItemSetsRepository { get; private set; }
    public ItemStats.ItemsStatsRepository ItemsStatsRepository { get; private set; }
    public Jobs.JobsRepository JobsRepository { get; private set; }
    public KnowledgeBook.KnowledgeBookRepository KnowledgeBookRepository { get; private set; }
    public Maps.MapsRepository MapsRepository { get; private set; }
    public Monsters.MonstersRepository MonstersRepository { get; private set; }
    public Names.NamesRepository NamesRepository { get; private set; }
    public Npcs.NpcsRepository NpcsRepository { get; private set; }
    public Pets.PetsRepository PetsRepository { get; private set; }
    public Pvp.PvpRepository PvpRepository { get; private set; }
    public Quests.QuestsRepository QuestsRepository { get; private set; }
    public Ranks.RanksRepository RanksRepository { get; private set; }
    public Rides.RidesRepository RidesRepository { get; private set; }
    public Runes.RunesRepository RunesRepository { get; private set; }
    public Scripts.ScriptsRepository ScriptsRepository { get; private set; }
    public Servers.ServersRepository ServersRepository { get; private set; }
    public Skills.SkillsRepository SkillsRepository { get; private set; }
    public SpeakingItems.SpeakingItemsRepository SpeakingItemsRepository { get; private set; }
    public Spells.SpellsRepository SpellsRepository { get; private set; }
    public States.StatesRepository StatesRepository { get; private set; }
    public TimeZone.TimeZoneRepository TimeZonesRepository { get; private set; }
    public Titles.TitlesRepository TitlesRepository { get; private set; }
    public TTG.TTGRepository TTGRepository { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DofusDatacenter"/> class.
    /// </summary>
    internal DofusDatacenter()
    {
        AlignmentsRepository = new();
        AudiosRepository = new();
        BreedsRepository = new();
        CraftsRepository = new();
        DialogsRepository = new();
        EffectsRepository = new();
        EmotesRepository = new();
        FightChallengesRepository = new();
        GuildsRepository = new();
        HintsRepository = new();
        HousesRepository = new();
        IncarnationsRepository = new();
        InteractiveObjectsRepository = new();
        ItemsRepository = new();
        ItemSetsRepository = new();
        ItemsStatsRepository = new();
        JobsRepository = new();
        KnowledgeBookRepository = new();
        MapsRepository = new();
        MonstersRepository = new();
        NamesRepository = new();
        NpcsRepository = new();
        PetsRepository = new();
        PvpRepository = new();
        QuestsRepository = new();
        RanksRepository = new();
        RidesRepository = new();
        RunesRepository = new();
        ScriptsRepository = new();
        ServersRepository = new();
        SkillsRepository = new();
        SpeakingItemsRepository = new();
        SpellsRepository = new();
        StatesRepository = new();
        TimeZonesRepository = new();
        TitlesRepository = new();
        TTGRepository = new();
    }

    /// <summary>
    /// Loads the specified lang type into the datacenter.
    /// </summary>
    /// <param name="type">The lang type to load.</param>
    public void Load(LangType type)
    {
        AlignmentsRepository = DofusRepository.Load<Alignments.AlignmentsRepository>(type);
        AudiosRepository = DofusRepository.Load<Audios.AudiosRepository>(type);
        BreedsRepository = DofusRepository.Load<Breeds.BreedsRepository>(type);
        CraftsRepository = DofusRepository.Load<Crafts.CraftsRepository>(type);
        DialogsRepository = DofusRepository.Load<Dialogs.DialogsRepository>(type);
        EffectsRepository = DofusRepository.Load<Effects.EffectsRepository>(type);
        EmotesRepository = DofusRepository.Load<Emotes.EmotesRepository>(type);
        FightChallengesRepository = DofusRepository.Load<FightChallenges.FightChallengesRepository>(type);
        GuildsRepository = DofusRepository.Load<Guilds.GuildsRepository>(type);
        HintsRepository = DofusRepository.Load<Hints.HintsRepository>(type);
        HousesRepository = DofusRepository.Load<Houses.HousesRepository>(type);
        IncarnationsRepository = DofusCustomRepository.Load<Incarnations.IncarnationsRepository>();
        InteractiveObjectsRepository = DofusRepository.Load<InteractiveObjects.InteractiveObjectsRepository>(type);
        ItemsRepository = DofusRepository.Load<Items.ItemsRepository>(type);
        ItemSetsRepository = DofusRepository.Load<ItemSets.ItemSetsRepository>(type);
        ItemsStatsRepository = DofusRepository.Load<ItemStats.ItemsStatsRepository>(type);
        JobsRepository = DofusRepository.Load<Jobs.JobsRepository>(type);
        KnowledgeBookRepository = DofusRepository.Load<KnowledgeBook.KnowledgeBookRepository>(type);
        MapsRepository = DofusRepository.Load<Maps.MapsRepository>(type);
        MonstersRepository = DofusRepository.Load<Monsters.MonstersRepository>(type);
        NamesRepository = DofusRepository.Load<Names.NamesRepository>(type);
        NpcsRepository = DofusRepository.Load<Npcs.NpcsRepository>(type);
        PetsRepository = DofusCustomRepository.Load<Pets.PetsRepository>();
        PvpRepository = DofusRepository.Load<Pvp.PvpRepository>(type);
        QuestsRepository = DofusRepository.Load<Quests.QuestsRepository>(type);
        RanksRepository = DofusRepository.Load<Ranks.RanksRepository>(type);
        RidesRepository = DofusRepository.Load<Rides.RidesRepository>(type);
        RunesRepository = DofusCustomRepository.Load<Runes.RunesRepository>();
        ScriptsRepository = DofusRepository.Load<Scripts.ScriptsRepository>(type);
        ServersRepository = DofusRepository.Load<Servers.ServersRepository>(type);
        SkillsRepository = DofusRepository.Load<Skills.SkillsRepository>(type);
        SpeakingItemsRepository = DofusRepository.Load<SpeakingItems.SpeakingItemsRepository>(type);
        SpellsRepository = DofusRepository.Load<Spells.SpellsRepository>(type);
        StatesRepository = DofusRepository.Load<States.StatesRepository>(type);
        TimeZonesRepository = DofusRepository.Load<TimeZone.TimeZoneRepository>(type);
        TitlesRepository = DofusRepository.Load<Titles.TitlesRepository>(type);
        TTGRepository = DofusRepository.Load<TTG.TTGRepository>(type);

        ImageUrlProvider.ClearCache();
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
}
