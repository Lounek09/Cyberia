using System.Collections.Frozen;

namespace Cyberia.Salamandra;

public static class Emojis
{
    public const string EffectHealthPoint = "<:effect_healthPoint:1238099013102735411>";
    public const string EffectAp = "<:effect_ap:1238098561363738715>";
    public const string EffectMp = "<:effect_mp:1238099051862294568>";
    public const string EffectApResistance = "<:effect_apResistance:1238098555315425383>";
    public const string EffectMpResistance = "<:effect_mpResistance:1238099053028442133>";
    public const string EffectNeutralResistance = "<:effect_neutralResistance:1238099064000614412>";
    public const string EffectEarthResistance = "<:effect_earthResistance:1238098650270400512>";
    public const string EffectFireResistance = "<:effect_fireResistance:1238098679739584582>";
    public const string EffectWaterResistance = "<:effect_waterResistance:1238099579938013249>";
    public const string EffectAirResistance = "<:effect_airResistance:1238098559451009146>";


    public static readonly FrozenDictionary<int, string> s_effects = new Dictionary<int, string>()
    {
        { 5, "<:effect_push:1238099091196612629>" },
        { 6, "<:effect_attract:1238098582914207784>" },
        { 7, "<:effect_divorce:1238098631718862860>" },
        { 10, "<:effect_emote:1238098646344400956>" },
        { 13, "<:effect_hourglass:1238098779668746281>" },
        { 34, "<:effect_book:1238098585292243015>" },
        { 35, "<:effect_book:1238098585292243015>" },
        { 50, "<:effect_carryThrow:1238098605538152488>" },
        { 51, "<:effect_carryThrow:1238098605538152488>" },
        { 77, EffectMp },
        { 78, EffectMp },
        { 79, "<:effect_dice:1238126936241209434>" },
        { 81, "<:effect_healthGain:1238099012054155295>" },
        { 82, "<:effect_neutralDammage:1238099049052373092>" },
        { 84, EffectAp },
        { 85, "<:effect_waterDammage:1238099578562281503>" },
        { 86, "<:effect_earthDammage:1238098648710123565>" },
        { 87, "<:effect_airDammage:1238098558230728707>" },
        { 88, "<:effect_fireDammage:1238142424174301287>" },
        { 89, "<:effect_neutralDammage:1238099049052373092>" },
        { 90, "<:effect_healthGain:1238099012054155295>" },
        { 91, "<:effect_waterDammage:1238099578562281503>" },
        { 92, "<:effect_earthDammage:1238098648710123565>" },
        { 93, "<:effect_airDammage:1238098558230728707>" },
        { 94, "<:effect_fireDammage:1238142424174301287>" },
        { 95, "<:effect_neutralDammage:1238099049052373092>" },
        { 96, "<:effect_waterDammage:1238099578562281503>" },
        { 97, "<:effect_earthDammage:1238098648710123565>" },
        { 98, "<:effect_airDammage:1238098558230728707>" },
        { 99, "<:effect_fireDammage:1238142424174301287>" },
        { 100, "<:effect_neutralDammage:1238099049052373092>" },
        { 101, EffectAp },
        { 105, "<:effect_shield:1238099128437833809>" },
        { 106, "<:effect_returnSpell:1238099113988460594>" },
        { 107, "<:effect_returnDammage:1238099112839221278>" },
        { 108, "<:effect_healthGain:1238099012054155295>" },
        { 110, "<:effect_healthGain:1238099012054155295>" },
        { 111, EffectAp },
        { 112, "<:effect_dammage:1238098618670649355>" },
        { 113, "<:effect_dice:1238126936241209434>" },
        { 114, "<:effect_power:1238099068488781865>" },
        { 115, "<:effect_crit:1238098622860623913>" },
        { 116, "<:effect_range:1238099092018565281>" },
        { 117, "<:effect_range:1238099092018565281>" },
        { 118, "<:effect_strength:1238099544919506976>" },
        { 119, "<:effect_agility:1238098556993147114>" },
        { 120, EffectAp },
        { 121, "<:effect_dammage:1238098618670649355>" },
        { 122, "<:effect_critFailure:1238098624093749330>" },
        { 123, "<:effect_chance:1238098600031031307>" },
        { 124, "<:effect_wisdom:1238099588313911366>" },
        { 125, "<:effect_health:1238099016428949574>" },
        { 126, "<:effect_intelligence:1238098814024548393>" },
        { 127, EffectMp },
        { 128, EffectMp },
        { 130, "<:effect_gold:1238098780956393605>" },
        { 132, "<:effect_dispell:1238098638018707478>" },
        { 133, EffectAp },
        { 134, EffectMp },
        { 135, "<:effect_range:1238099092018565281>" },
        { 136, "<:effect_range:1238099092018565281>" },
        { 137, "<:effect_physicalDammage:1238099065221283863>" },
        { 138, "<:effect_power:1238099068488781865>" },
        { 139, "<:effect_energy:1238098670717505537>" },
        { 140, "<:effect_sleep:1238099529606103061>" },
        { 141, "<:effect_kill:1238099015287967774>" },
        { 142, "<:effect_physicalDammage:1238099065221283863>" },
        { 143, "<:effect_healthGain:1238099012054155295>" },
        { 145, "<:effect_dammage:1238098618670649355>" },
        { 146, "<:effect_talk:1238099547259928646>" },
        { 147, "<:effect_reborn:1238099093474246738>" },
        { 148, "<:effect_follower:1238098721238155354>" },
        { 152, "<:effect_chance:1238098600031031307>" },
        { 153, "<:effect_health:1238099016428949574>" },
        { 154, "<:effect_agility:1238098556993147114>" },
        { 155, "<:effect_intelligence:1238098814024548393>" },
        { 156, "<:effect_wisdom:1238099588313911366>" },
        { 157, "<:effect_strength:1238099544919506976>" },
        { 158, "<:effect_weight:1238099587240296480>" },
        { 159, "<:effect_weight:1238099587240296480>" },
        { 160, EffectApResistance },
        { 161, EffectMpResistance },
        { 162, EffectApResistance },
        { 163, EffectMpResistance },
        { 164, "<:effect_shield:1238099128437833809>" },
        { 165, "<:effect_weapon:1238099574296674324>" },
        { 166, EffectAp },
        { 168, EffectAp },
        { 169, EffectMp },
        { 171, "<:effect_crit:1238098622860623913>" },
        { 172, "<:effect_magicalResistance:1238099036263677983>" },
        { 173, "<:effect_physicalResistance:1238099066559397978>" },
        { 174, "<:effect_initiative:1238098795338928189>" },
        { 175, "<:effect_initiative:1238098795338928189>" },
        { 176, "<:effect_prospecting:1238099089787322428>" },
        { 177, "<:effect_prospecting:1238099089787322428>" },
        { 178, "<:effect_heal:1238098784651575317>" },
        { 179, "<:effect_heal:1238098784651575317>" },
        { 181, "<:effect_creature:1238098621317120051>" },
        { 182, "<:effect_summon:1238099546035322990>" },
        { 183, "<:effect_magicalResistance:1238099036263677983>" },
        { 184, "<:effect_physicalResistance:1238099066559397978>" },
        { 185, "<:effect_staticCreature:1238099528607993909>" },
        { 186, "<:effect_power:1238099068488781865>" },
        { 188, "<:effect_demonAngel:1238098633623076874>" },
        { 192, "<:effect_bin:1238098583832629249>" },
        { 193, "<:effect_bag:1240405208685609080>" },
        { 194, "<:effect_kamas:1238098812632039425>" },
        { 206, "<:effect_reborn:1238099093474246738>" },
        { 208, "<:effect_spell:1238099532173283328>" },
        { 210, EffectEarthResistance },
        { 211, EffectWaterResistance },
        { 212, EffectAirResistance },
        { 213, EffectFireResistance },
        { 214, EffectNeutralResistance },
        { 215, EffectEarthResistance },
        { 216, EffectWaterResistance },
        { 217, EffectAirResistance },
        { 218, EffectFireResistance },
        { 219, EffectNeutralResistance },
        { 220, "<:effect_returnDammage:1238099112839221278>" },
        { 221, "<:effect_gift:1238098719967150212>" },
        { 222, "<:effect_gift:1238098719967150212>" },
        { 225, "<:effect_trapDammage:1238099560296091748>" },
        { 226, "<:effect_trapPowerDammage:1238099561646522479>" },
        { 228, "<:effect_spell:1238099532173283328>" },
        { 229, "<:effect_ride:1238099108611489843>" },
        { 230, "<:effect_energy:1238098670717505537>" },
        { 233, "<:effect_bin:1238098583832629249>" },
        { 240, EffectEarthResistance },
        { 241, EffectWaterResistance },
        { 242, EffectAirResistance },
        { 243, EffectFireResistance },
        { 244, EffectNeutralResistance },
        { 245, EffectEarthResistance },
        { 246, EffectWaterResistance },
        { 247, EffectAirResistance },
        { 248, EffectFireResistance },
        { 249, EffectNeutralResistance },
        { 250, EffectEarthResistance },
        { 251, EffectWaterResistance },
        { 252, EffectAirResistance },
        { 253, EffectFireResistance },
        { 254, EffectNeutralResistance },
        { 255, EffectEarthResistance },
        { 256, EffectWaterResistance },
        { 257, EffectAirResistance },
        { 258, EffectFireResistance },
        { 259, EffectNeutralResistance },
        { 260, EffectEarthResistance },
        { 261, EffectWaterResistance },
        { 262, EffectAirResistance },
        { 263, EffectFireResistance },
        { 264, EffectNeutralResistance },
        { 265, "<:effect_shield:1238099128437833809>" },
        { 266, "<:effect_chance:1238098600031031307>" },
        { 267, "<:effect_health:1238099016428949574>" },
        { 268, "<:effect_agility:1238098556993147114>" },
        { 269, "<:effect_intelligence:1238098814024548393>" },
        { 270, "<:effect_wisdom:1238099588313911366>" },
        { 271, "<:effect_strength:1238099544919506976>" },
        { 275, "<:effect_waterDammage:1238099578562281503>" },
        { 276, "<:effect_earthDammage:1238098648710123565>" },
        { 277, "<:effect_airDammage:1238098558230728707>" },
        { 278, "<:effect_fireDammage:1238142424174301287>" },
        { 279, "<:effect_neutralDammage:1238099049052373092>" },
        { 280, "<:effect_enhanceRange:1238098664812183663>" },
        { 281, "<:effect_enhanceRange:1238098664812183663>" },
        { 282, "<:effect_enableRange:1238098666703552532>" },
        { 283, "<:effect_boostDamage:1238098581852913674>" },
        { 284, "<:effect_boostHeal:1238098601218146335>" },
        { 285, "<:effect_reduceApCost:1238099088348807321>" },
        { 286, "<:effect_enhanceCastSpeed:1238098683560460411>" },
        { 287, "<:effect_boostCrit:1238098586986741900>" },
        { 289, "<:effect_disableSight:1238098636710088724>" },
        { 290, "<:effect_enhanceCastPerTurn:1238098682071486578>" },
        { 291, "<:effect_enhanceCastPerTarget:1238098681106796666>" },
        { 293, "<:effect_boostDamage:1238098581852913674>" },
        { 294, "<:effect_reduceRange:1238099110167580692>" },
        { 295, "<:effect_reduceRange:1238099110167580692>" },
        { 296, "<:effect_increaseApCost:1238098800703307777>" },
        { 300, "<:effect_spell:1238099532173283328>" },
        { 320, "<:effect_range:1238099092018565281>" },
        { 333, "<:effect_color:1238098619928674355>" },
        { 335, "<:effect_color:1238098619928674355>" },
        { 400, "<:effect_trap:1238099558681153597>" },
        { 405, "<:effect_kill:1238099015287967774>" },
        { 406, "<:effect_dispell:1238098638018707478>" },
        { 513, "<:effect_prism:1238099062792912928>" },
        { 600, "<:effect_zaap:1238099586086866994>" },
        { 601, "<:effect_tpToMap:1238099543078469695>" },
        { 602, "<:effect_zaap:1238099586086866994>" },
        { 603, "<:effect_job:1238098816780079176>" },
        { 604, "<:effect_spell:1238099532173283328>" },
        { 605, "<:effect_xp:1238099604965429292>" },
        { 606, "<:effect_wisdom:1238099588313911366>" },
        { 607, "<:effect_strength:1238099544919506976>" },
        { 608, "<:effect_chance:1238098600031031307>" },
        { 609, "<:effect_agility:1238098556993147114>" },
        { 610, "<:effect_health:1238099016428949574>" },
        { 611, "<:effect_intelligence:1238098814024548393>" },
        { 612, "<:effect_caracteristic:1238098602476441621>" },
        { 613, "<:effect_spell:1238099532173283328>" },
        { 614, "<:effect_jobXp:1238098818164064307>" },
        { 615, "<:effect_forgotJob:1238098722244661259>" },
        { 616, "<:effect_spell:1238099532173283328>" },
        { 620, "<:effect_book:1238098585292243015>" },
        { 621, "<:effect_egg:1238098651717570611>" },
        { 622, "<:effect_house:1238098796815056937>" },
        { 623, "<:effect_soul:1238099531086827622>" },
        { 624, "<:effect_spell:1238099532173283328>" },
        { 626, "<:effect_caracteristic:1238098602476441621>" },
        { 627, "<:effect_map:1238099031557935134>" },
        { 628, "<:effect_soul:1238099531086827622>" },
        { 629, "<:effect_spell:1238099532173283328>" },
        { 640, "<:effect_demonAngel:1238098633623076874>" },
        { 641, "<:effect_demonAngel:1238098633623076874>" },
        { 642, "<:effect_demonAngel:1238098633623076874>" },
        { 643, "<:effect_demonAngel:1238098633623076874>" },
        { 645, "<:effect_reborn:1238099093474246738>" },
        { 646, "<:effect_healthGain:1238099012054155295>" },
        { 647, "<:effect_ghost:1238098724152934511>" },
        { 648, "<:effect_ghost:1238098724152934511>" },
        { 649, "<:effect_demonAngel:1238098633623076874>" },
        { 669, "<:effect_incarnation:1238098799570718720>" },
        { 670, "<:effect_neutralDammage:1238099049052373092>" },
        { 671, "<:effect_neutralDammage:1238099049052373092>" },
        { 672, "<:effect_neutralDammage:1238099049052373092>" },
        { 699, "<:effect_linkJob:1238099033189384262>" },
        { 702, "<:effect_itemResistance:1238098815677108265>" },
        { 705, "<:effect_soul:1238099531086827622>" },
        { 706, "<:effect_ride:1238099108611489843>" },
        { 721, "<:effect_ghoul:1238098725851889754>" },
        { 722, "<:effect_spell:1238099532173283328>" },
        { 723, "<:effect_emote:1238098646344400956>" },
        { 724, "<:effect_title:1238099548090662996>" },
        { 725, "<:effect_guild:1238098782009430056>" },
        { 730, "<:effect_prism:1238099062792912928>" },
        { 731, "<:effect_demonAngel:1238098633623076874>" },
        { 740, "<:effect_shushu:1238099129763106826>" },
        { 741, "<:effect_shushu:1238099129763106826>" },
        { 742, "<:effect_shushu:1238099129763106826>" },
        { 750, "<:effect_soul:1238099531086827622>" },
        { 751, "<:effect_ride:1238099108611489843>" },
        { 770, "<:effect_hourglass:1238098779668746281>" },
        { 771, "<:effect_hourglass:1238098779668746281>" },
        { 772, "<:effect_hourglass:1238098779668746281>" },
        { 773, "<:effect_hourglass:1238098779668746281>" },
        { 775, "<:effect_hourglass:1238098779668746281>" },
        { 776, "<:effect_erosion:1238099111538987038>" },
        { 780, "<:effect_reborn:1238099093474246738>" },
        { 781, "<:effect_dice:1238126936241209434>" },
        { 782, "<:effect_dice:1238126936241209434>" },
        { 783, "<:effect_push:1238099091196612629>" },
        { 786, "<:effect_healthGain:1238099012054155295>" },
        { 787, "<:effect_spell:1238099532173283328>" },
        { 791, "<:effect_scroll:1238099125686374511>" },
        { 795, "<:effect_hunt:1238098798295650326>" },
        { 800, EffectHealthPoint },
        { 811, "<:effect_hourglass:1238098779668746281>" },
        { 812, "<:effect_itemResistance:1238098815677108265>" },
        { 814, "<:effect_key:1238099014310826005>" },
        { 825, "<:effect_tpToMap:1238099543078469695>" },
        { 826, "<:effect_tpToMap:1238099543078469695>" },
        { 830, "<:effect_guild:1238098782009430056>" },
        { 850, "<:effect_name:1238099054270087229>" },
        { 851, "<:effect_color:1238098619928674355>" },
        { 852, "<:effect_sex:1238099127016099891>" },
        { 853, "<:effect_mimisymbic:1238099050587488326>" },
        { 856, "<:effect_ttgBinder:1238099562657353860>" },
        { 905, "<:effect_egg:1238098651717570611>" },
        { 930, "<:effect_serenity:1238098537439559751>" },
        { 931, "<:effect_aggressiveness:1238098539108634654>" },
        { 932, "<:effect_stamina:1238098540379635712>" },
        { 933, "<:effect_stamina:1238098540379635712>" },
        { 934, "<:effect_love:1238098541520359516>" },
        { 935, "<:effect_love:1238098541520359516>" },
        { 936, "<:effect_maturity:1238098535623163985>" },
        { 937, "<:effect_maturity:1238098535623163985>" },
        { 939, "<:effect_dna:1238098647392981054>" },
        { 940, "<:effect_dna:1238098647392981054>" },
        { 945, "<:effect_dna:1238098647392981054>" },
        { 946, "<:effect_enclosureAction:1238098669475987577>" },
        { 947, "<:effect_enclosureAction:1238098669475987577>" },
        { 948, "<:effect_enclosure:1238098667961847870>" },
        { 949, "<:effect_ride:1238099108611489843>" },
        { 950, "<:effect_state:1238099527404093440>" },
        { 951, "<:effect_state:1238099527404093440>" },
        { 960, "<:effect_demonAngel:1238098633623076874>" },
        { 961, "<:effect_demonAngel:1238098633623076874>" },
        { 969, "<:effect_mimisymbic:1238099050587488326>" },
        { 974, "<:effect_xp:1238099604965429292>" },
        { 983, "<:effect_lock:1238099034888077322>" },
        { 985, "<:effect_sign:1238099123962380380>" },
        { 986, "<:effect_scroll:1238099125686374511>" },
        { 987, "<:effect_sign:1238099123962380380>" },
        { 988, "<:effect_sign:1238099123962380380>" },
        { 989, "<:effect_magnifier:1238099039149625405>" },
        { 994, "<:effect_warning:1238099576917852231>" },
        { 995, "<:effect_hand:1238098783209000991>" },
        { 999, "<:effect_tpToMap:1238099543078469695>" },
        { 2001, "<:effect_kamas:1238098812632039425>" },
        { 2008, "<:effect_power:1238099068488781865>" },
        { 2009, "<:effect_power:1238099068488781865>" },
        { 2050, "<:effect_xp:1238099604965429292>" },
        { 2100, EffectAp },
        { 2101, "<:effect_ttgBooster:1238099557250891827>" },
        { 2102, "<:effect_ttgCard:1238099575546581003>" },
        { 2107, "<:effect_ttgBooster:1238099557250891827>" },
        { 2111, "<:effect_returnDammage:1238099112839221278>" },
        { 2112, "<:effect_summon:1238099546035322990>" },
        { 2113, "<:effect_trapPowerDammage:1238099561646522479>" },
        { 2114, "<:effect_trapDammage:1238099560296091748>" },
        { 2118, "<:effect_healthGain:1238099012054155295>" },
        { 2123, "<:effect_reduceApCost:1238099088348807321>" },
        { 2124, "<:effect_enhanceRange:1238098664812183663>" },
        { 2127, "<:effect_attract:1238098582914207784>" },
        { 2128, "<:effect_state:1238099527404093440>" },
        { 2129, "<:effect_state:1238099527404093440>" },
        { 2130, "<:effect_zaap:1238099586086866994>" },
        { 2132, "<:effect_card:1238098604174872648>" },
        { 2133, "<:effect_card:1238098604174872648>" },
        { 2136, "<:effect_neutralDammage:1238099049052373092>" },
        { 2137, "<:effect_state:1238099527404093440>" },
        { 2138, "<:effect_boostDamage:1238098581852913674>" },
        { 2149, "<:effect_itemResistance:1238098815677108265>" },
        { 2151, "<:effect_lock:1238099034888077322>" },
        { 2152, "<:effect_bag:1240405208685609080>" },
        { 2153, "<:effect_lock2:1252190910213787650>" },
        { 2154, "<:effect_lock2:1252190910213787650>" },
        { 2155, "<:effect_lock2:1252190910213787650>" }
    }.ToFrozenDictionary();

    public static string Effect(int id)
    {
        return s_effects.TryGetValue(id, out var emoji) ? emoji : Empty;
    }

    //Areas
    private static readonly FrozenDictionary<int, string> s_effectAreas = new SortedDictionary<int, string>()
    {
        { 1, "<:effectarea_cross:1238078599144144956>" },
        { 2, "<:effectarea_perpendicular_line:1238078621512503328>" },
        { 3, "<:effectarea_circle:1238078596476702771>" },
        { 4, "<:effectarea_dot:1238078601736491048>" },
        { 5, "<:effectarea_line:1238078625547292735>" },
        { 6, "<:effectarea_checked_patern:1238078595419738144>" },
        { 7, "<:effectarea_ring:1238078624528203856>" },
        { 8, "<:effectarea_square:1238078635689115738>" },
        { 9, "<:effectarea_void_square:1238078644765851750>" },
        { 10, "<:effectarea_cone:1238078598020075580>" },
        { 11, "<:effectarea_diagonal_cross:1238078600360624158>" },
        { 12, "<:effectarea_t:1238078638772064296>" },
        { 13, "<:effectarea_t:1238078638772064296>" },
        { 15, "<:effectarea_void_cross:1238078640571289622>" },
        { 16, "<:effectarea_large_line:1238078594022903819>" },
        { 17, "<:effectarea_point:1238078622829514772>" },
        { 21, "<:effectarea_star:1238078637056725043>" },
        { 23, "<:effectarea_fork:1238078592911675423>" },
        { 67, "<:effectarea_circle:1238078596476702771>" },
        { 68, "<:effectarea_checked_patern:1238078595419738144>" },
        { 76, "<:effectarea_line:1238078625547292735>" },
        { 79, "<:effectarea_ring:1238078624528203856>" },
        { 80, "<:effectarea_dot:1238078601736491048>" },
        { 84, "<:effectarea_perpendicular_line:1238078621512503328>" },
        { 88, "<:effectarea_cross:1238078599144144956>" }
    }.ToFrozenDictionary();

    public static string EffectArea(int id)
    {
        return s_effectAreas.TryGetValue(id, out var emoji) ? emoji : Unknown;
    }

    //Runes
    private static readonly FrozenDictionary<int, string> s_baRunes = new Dictionary<int, string>()
    {
        { 1, "<:rune_fo:1238075386387234847>" },
        { 2, "<:rune_sa:1238075626095771699>" },
        { 3, "<:rune_ine:1238075428510634036>" },
        { 4, "<:rune_vi:1238075719238549504>" },
        { 5, "<:rune_age:1238075332091838527>" },
        { 6, "<:rune_cha:1238075336252457063>" },
        { 7, "<:rune_ga_pa:1238075387800453130>" },
        { 8, "<:rune_ga_pme:1238075389251944488>" },
        { 9, "<:rune_cri:1238075330607054949>" },
        { 10, "<:rune_so:1238075627714641990>" },
        { 11, "<:rune_do:1238075357345878017>" },
        { 12, "<:rune_do_per:1238075359799414905>" },
        { 13, "<:rune_do_ren:1238075361426669629>" },
        { 14, "<:rune_po:1238075528053919785>" },
        { 15, "<:rune_summo:1238075622048403537>" },
        { 16, "<:rune_pod:1238075560521891920>" },
        { 17, "<:rune_pi:1238075531585519656>" },
        { 18, "<:rune_pi_per:1238075526443307110>" },
        { 19, "<:rune_ini:1238075429768925204>" },
        { 20, "<:rune_prospe:1238075553911803924>" },
        { 21, "<:rune_fire_re:1238075390468034590>" },
        { 22, "<:rune_air_re:1238075333127831663>" },
        { 23, "<:rune_water_re:1238075720429867008>" },
        { 24, "<:rune_earth_re:1238075355374551110>" },
        { 25, "<:rune_neutral_re:1238075431106908160>" },
        { 26, "<:rune_fire_re_per:1238075391676121260>" },
        { 27, "<:rune_air_re_per:1238075334885244928>" },
        { 28, "<:rune_earth_re_per:1238075356502822924>" },
        { 29, "<:rune_neutral_re_per:1238075432062947371>" },
        { 30, "<:rune_water_re_per:1238075717959422002>" },
        { 31, "<:rune_hunting:1238075433602256976>" }
    }.ToFrozenDictionary();

    public static string BaRune(int id)
    {
        return s_baRunes.TryGetValue(id, out var emoji) ? emoji : Empty;
    }

    private static readonly FrozenDictionary<int, string> s_paRunes = new Dictionary<int, string>()
    {
        { 1, "<:rune_pa_fo:1238075466649309235>" },
        { 2, "<:rune_pa_sa:1238075529060417567>" },
        { 3, "<:rune_pa_ine:1238075461028806717>" },
        { 4, "<:rune_pa_vi:1238075530453192756>" },
        { 5, "<:rune_pa_age:1238075462295486464>" },
        { 6, "<:rune_pa_cha:1238075463528747098>" },
        { 12, "<:rune_pa_do_per:1238075465298608139>" },
        { 16, "<:rune_pa_pod:1238075494843289640>" },
        { 17, "<:rune_pa_pi:1238075499553488976>" },
        { 18, "<:rune_pa_pi_per:1238075500648464414>" },
        { 19, "<:rune_pa_ini:1238075497544548404>" },
        { 20, "<:rune_pa_prospe:1238075496349171732>" }
    }.ToFrozenDictionary();

    public static string PaRune(int id)
    {
        return s_paRunes.TryGetValue(id, out var emoji) ? emoji : Empty;
    }

    private static readonly FrozenDictionary<int, string> s_raRunes = new Dictionary<int, string>()
    {
        { 1, "<:rune_ra_fo:1238075593220685866>" },
        { 2, "<:rune_ra_sa:1238075626095771699>" },
        { 3, "<:rune_ra_ine:1238075594537832549>" },
        { 4, "<:rune_ra_vi:1238075624535621702>" },
        { 5, "<:rune_ra_age:1238075556743086121>" },
        { 6, "<:rune_ra_cha:1238075558051577967>" },
        { 12, "<:rune_ra_do_per:1238075559339229194>" },
        { 16, "<:rune_ra_pod:1238075591689900133>" },
        { 18, "<:rune_ra_pi_per:1238075597658525826>" },
        { 19, "<:rune_ra_ini:1238075596060364841>" }
    }.ToFrozenDictionary();

    public static string RaRune(int id)
    {
        return s_raRunes.TryGetValue(id, out var emoji) ? emoji : Empty;
    }

    //Bool
    private const string c_true = "<:true:1238073348672585758>";
    private const string c_false = "<:false:1238073407845826681>";

    public static string Bool(bool value)
    {
        return value ? c_true : c_false;
    }

    //Quests
    private const string c_quest = "<:quest:1238073346391015474>";
    private const string c_repeatableQuest = "<:repeatable_quest:1238073347187802174>";
    private const string c_accountQuest = "<:account_quest:1238073485918605343>";

    public static string Quest(bool isRepeatable, bool isAccount)
    {
        if (isRepeatable && isAccount)
        {
            return c_repeatableQuest + c_accountQuest;
        }

        if (isAccount)
        {
            return c_accountQuest;
        }

        if (isRepeatable)
        {
            return c_repeatableQuest;
        }

        return c_quest;
    }

    //Others
    public const string Empty = "<:empty:1238073357690343434>";
    public const string House = "<:house:1238073343899598958>";
    public const string Dungeon = "<:dungeon:1238073355039670312>";
    public const string Kamas = "<:kamas:1238073345229193217>";
    public const string Xp = "<:xp:1238073351025594408>";
    public const string Unknown = "<:unknown:1238073349683544099>";
}
