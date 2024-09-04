using Cyberia.Langzilla.Enums;

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
    public Names.NamesRepository NamesRepository { get; init; }
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
    public TimeZone.TimeZoneRepository TimeZonesRepository { get; init; }
    public Titles.TitlesRepository TitlesRepository { get; init; }
    public TTG.TTGRepository TTGRepository { get; init; }

    internal Datacenter(LangType type)
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
