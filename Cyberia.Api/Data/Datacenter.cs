using System.Text.Json;

namespace Cyberia.Api.Data
{
    public sealed class Datacenter
    {
        public AlignmentsData AlignmentsData { get; init; }
        public AudioData AudiosData { get; init; }
        public BreedsData BreedsData { get; init; }
        public CraftsData CraftsData { get; init; }
        public DialogsData DialogsData { get; init; }
        public EffectsData EffectsData { get; init; }
        public EmotesData EmotesData { get; init; }
        public FightChallengesData FightChallengesData { get; init; }
        public GuildsData GuildsData { get; init; }
        public HintsData HintsData { get; init; }
        public HousesData HousesData { get; init; }
        public IncarnationsData IncarnationsData { get; init; }
        public InteractiveObjectsData InteractiveObjectsData { get; init; }
        public ItemsData ItemsData { get; init; }
        public ItemSetsData ItemSetsData { get; init; }
        public ItemsStatsData ItemsStatsData { get; init; }
        public JobsData JobsData { get; init; }
        public KnowledgeBookData KnowledgeBookData { get; init; }
        public MapsData MapsData { get; init; }
        public MonstersData MonstersData { get; init; }
        public NpcsData NpcsData { get; init; }
        public PetsData PetsData { get; init; }
        public PvpData PvpData { get; init; }
        public QuestsData QuestsData { get; init; }
        public RanksData RanksData { get; init; }
        public RidesData RidesData { get; init; }
        public RunesData RunesData { get; init; }
        public ScriptsData ScriptsData { get; init; }
        public ServersData ServersData { get; init; }
        public SkillsData SkillsData { get; init; }
        public SpeakingItemsData SpeakingItemsData { get; init; }
        public SpellsData SpellsData { get; init; }
        public StatesData StatesData { get; init; }
        public TaxCollectorNamesData TaxCollectorNamesData { get; init; }
        public TimeZonesData TimeZonesData { get; init; }
        public TitlesData TitlesData { get; init; }
        public TTGData TTGData { get; init; }

        public Datacenter()
        {
            AlignmentsData = AlignmentsData.Load();
            AudiosData = AudioData.Load();
            BreedsData = BreedsData.Load();
            CraftsData = CraftsData.Load();
            DialogsData = DialogsData.Load();
            EffectsData = EffectsData.Load();
            EmotesData = EmotesData.Load();
            FightChallengesData = FightChallengesData.Load();
            GuildsData = GuildsData.Load();
            HintsData = HintsData.Load();
            HousesData = HousesData.Load();
            IncarnationsData = IncarnationsData.Load();
            InteractiveObjectsData = InteractiveObjectsData.Load();
            ItemsData = ItemsData.Load();
            ItemSetsData = ItemSetsData.Load();
            ItemsStatsData = ItemsStatsData.Load();
            JobsData = JobsData.Load();
            KnowledgeBookData = KnowledgeBookData.Load();
            MapsData = MapsData.Load();
            MonstersData = MonstersData.Load();
            NpcsData = NpcsData.Load();
            PetsData = PetsData.Load();
            PvpData = PvpData.Load();
            QuestsData = QuestsData.Load();
            RanksData = RanksData.Load();
            RidesData = RidesData.Load();
            RunesData = RunesData.Load();
            ScriptsData = ScriptsData.Load();
            ServersData = ServersData.Load();
            SkillsData = SkillsData.Load();
            SpeakingItemsData = SpeakingItemsData.Load();
            SpellsData = SpellsData.Load();
            StatesData = StatesData.Load();
            TaxCollectorNamesData = TaxCollectorNamesData.Load();
            TimeZonesData = TimeZonesData.Load();
            TitlesData = TitlesData.Load();
            TTGData = TTGData.Load();
        }
    }
}
