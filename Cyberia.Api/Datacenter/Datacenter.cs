namespace Cyberia.Api.DatacenterNS
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
            AlignmentsData = AlignmentsData.Build();
            AudiosData = AudioData.Build();
            BreedsData = BreedsData.Build();
            CraftsData = CraftsData.Build();
            DialogsData = DialogsData.Build();
            EffectsData = EffectsData.Build();
            EmotesData = EmotesData.Build();
            FightChallengesData = FightChallengesData.Build();
            GuildsData = GuildsData.Build();
            HintsData = HintsData.Build();
            HousesData = HousesData.Build();
            IncarnationsData = IncarnationsData.Build();
            InteractiveObjectsData = InteractiveObjectsData.Build();
            ItemsData = ItemsData.Build();
            ItemSetsData = ItemSetsData.Build();
            ItemsStatsData = ItemsStatsData.Build();
            JobsData = JobsData.Build();
            KnowledgeBookData = KnowledgeBookData.Build();
            MapsData = MapsData.Build();
            MonstersData = MonstersData.Build();
            NpcsData = NpcsData.Build();
            PetsData = PetsData.Build();
            PvpData = PvpData.Build();
            QuestsData = QuestsData.Build();
            RanksData = RanksData.Build();
            RidesData = RidesData.Build();
            RunesData = RunesData.Build();
            ScriptsData = ScriptsData.Build();
            ServersData = ServersData.Build();
            SkillsData = SkillsData.Build();
            SpeakingItemsData = SpeakingItemsData.Build();
            SpellsData = SpellsData.Build();
            StatesData = StatesData.Build();
            TaxCollectorNamesData = TaxCollectorNamesData.Build();
            TimeZonesData = TimeZonesData.Build();
            TitlesData = TitlesData.Build();
            TTGData = TTGData.Build();
        }
    }
}
