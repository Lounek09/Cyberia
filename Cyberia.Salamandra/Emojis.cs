namespace Cyberia.Salamandra
{
    public static class Emojis
    {
        //Effects
        public const string TRAP_PERCENT = "<:trapPercent:801770569695232025>";
        public const string TRAP_DAMAGE = "<:trapDamage:801770569556164678>";
        public const string TITLE = "<:title:801770569213280297>";
        public const string SUMMONABLE_CREATURES = "<:summonableCreatures:801770569242247218>";
        public const string STRENGHT = "<:strength:801770568734081036>";
        public const string STEAL_KAMAS = "<:stealsKamas:801770568834744382>";
        public const string SPELL_DAMMAGE = "<:spellDamage:801770568436678687>";
        public const string SIGHT = "<:sight:801770568282144789>";
        public const string RETURN = "<:return:801770568319369217>";
        public const string RES_WATER = "<:res_water:801770568348991498>";
        public const string RES_NEUTRAL = "<:res_neutral:801770568306655272>";
        public const string RES_FIRE = "<:res_fire:801770568453062716>";
        public const string RES_EARTH = "<:res_earth:801770568004665344>";
        public const string RES_AIR = "<:res_air:801770567954071602>";
        public const string RANGE = "<:range:801770567937163264>";
        public const string PUSH = "<:push:801770567585103912>";
        public const string PROSPECTING = "<:prospecting:801770567589167145>";
        public const string WEIGHT = "<:pods:801770567480770580>";
        public const string NEUTRAL_DAMAGE = "<:neutralDamage:801770566963953727>";
        public const string NEUTRAL = "<:neutral:801770567258603561>";
        public const string MOVEMENT_POINTS = "<:movementPoints:801770567275511810>";
        public const string LUCK = "<:luck:801770566863814677>";
        public const string LANGUAGE = "<:language:801770566662357003>";
        public const string INTELLIGENCE = "<:intelligence:801770566872203314>";
        public const string INCARNATION = "<:incarnation:801770566695649290>";
        public const string INITIATIVE = "<:initiative:801770566716489738>";
        public const string HUNTING = "<:hunting:801770566625263638>";
        public const string HEALTH = "<:health:801770566469156895>";
        public const string HEAL = "<:heal:801770566389334056>";
        public const string FOLLOWER = "<:follower:801770566192988180>";
        public const string FIRE_DAMAGE = "<:fireDamage:801770565773819937>";
        public const string ENHANCE_RANGE = "<:enhanceRange:801770566104907777>";
        public const string ENERGY = "<:energy:801770565903843328>";
        public const string ENABLE_RANGE = "<:enableRange:801770566121816114>";
        public const string EMOTE = "<:emote:801770565605916692>";
        public const string EARTH_DAMAGE = "<:earthDamage:801770565873827842>";
        public const string DODGE_MP = "<:dodgeMP:801770565304188979>";
        public const string DODGE_AP = "<:dodgeAP:801770565173772289>";
        public const string DAMAGES_PERCENT = "<:damagesPercent:801770565344952331>";
        public const string DAMAGE = "<:damage:801770564921982987>";
        public const string CRIT = "<:crit:801770565059870720>";
        public const string CAST_SPEED = "<:castSpeed:801770564627988490>";
        public const string CAST_PER_TURN = "<:castPerTurn:801770564595351572>";
        public const string AP_COST_REDUCTION = "<:apCostReduction:801770564238049290>";
        public const string AIR_DAMAGE = "<:airDamage:801770564141973515>";
        public const string AGILITY = "<:agility:801770564058349618>";
        public const string ACTION_POINTS = "<:actionPoints:801770563751772201>";
        public const string WISDOM = "<:wisdom:801770749190471680>";
        public const string WEAPON_DAMAGE = "<:weaponDamage:801770749052190720>";
        public const string WATER_DAMAGE = "<:waterDamage:801770748750331945>";
        public const string VITALITY = "<:vitality:801770748820979723>";
        public const string ATTACK_AP = "<:attackAP:804746110463508493>";
        public const string ATTACK_MP = "<:attackMP:804746109976838196>";
        public const string DEAD = "<:dead:804459730163597313>";

        public static string Effect(int id)
        {
            return id switch
            {
                226 => TRAP_PERCENT,
                225 => TRAP_DAMAGE,
                724 => TITLE,
                182 or 780 => SUMMONABLE_CREATURES,
                118 or 157 or 271 or 607 => STRENGHT,
                130 => STEAL_KAMAS,
                283 or 293 => SPELL_DAMMAGE,
                289 => SIGHT,
                106 or 220 => RETURN,
                211 or 216 or 241 or 246 or 251 or 256 or 261 => RES_WATER,
                214 or 219 or 244 or 249 or 254 or 259 or 264 => RES_NEUTRAL,
                213 or 218 or 243 or 248 or 253 or 258 or 263 => RES_FIRE,
                210 or 215 or 240 or 245 or 250 or 255 or 260 => RES_EARTH,
                212 or 217 or 242 or 247 or 252 or 257 or 262 => RES_AIR,
                116 or 117 or 135 or 136 or 320 => RANGE,
                5 or 6 or 783 => PUSH,
                176 or 177 => PROSPECTING,
                158 or 159 => WEIGHT,
                89 or 95 or 100 or 279 or 670 or 671 or 672 => NEUTRAL_DAMAGE,
                77 or 78 or 127 or 128 or 134 or 169 => MOVEMENT_POINTS,
                123 or 152 or 266 or 608 => LUCK,
                146 => LANGUAGE,
                126 or 155 or 269 or 611 => INTELLIGENCE,
                669 => INCARNATION,
                174 or 175 => INITIATIVE,
                795 => HUNTING,
                110 or 800 => HEALTH,
                81 or 108 or 143 or 178 or 179 or 284 or 646 => HEAL,
                148 => FOLLOWER,
                88 or 94 or 99 or 278 => FIRE_DAMAGE,
                281 or 294 => ENHANCE_RANGE,
                139 or 230 => ENERGY,
                282 => ENABLE_RANGE,
                10 => EMOTE,
                86 or 92 or 97 or 276 => EARTH_DAMAGE,
                161 => DODGE_MP,
                160 => DODGE_AP,
                138 or 186 => DAMAGES_PERCENT,
                112 or 121 or 145 => DAMAGE,
                115 or 171 or 287 => CRIT,
                286 => CAST_SPEED,
                290 => CAST_PER_TURN,
                285 => AP_COST_REDUCTION,
                87 or 93 or 98 or 277 => AIR_DAMAGE,
                119 or 154 or 268 or 609 => AGILITY,
                84 or 101 or 111 or 120 or 133 or 168 or 2100 => ACTION_POINTS,
                124 or 156 or 270 or 606 => WISDOM,
                165 => WEAPON_DAMAGE,
                85 or 91 or 96 or 275 => WATER_DAMAGE,
                125 or 153 or 267 or 610 => VITALITY,
                141 => DEAD,
                163 => ATTACK_MP,
                162 or 166 => ATTACK_AP,
                _ => EMPTY,
            };
        }


        //States
        public static readonly Dictionary<int, string> STATES = new()
        {
            { 0, "<:state_neutral:1067793405855408128>" },
            { 1, "<:state_drunk:1067793252717170748>" },
            { 2, "<:state_soul_seeker:1067808312122413086>" },
            { 3, "<:state_carrying:1067793148723597352>" },
            { 5, "<:state_disorient:1067793251035271199>" },
            { 6, "<:state_rooted:1067808274424004648>" },
            { 7, "<:state_gravity:1067793310372069466>" },
            { 8, "<:state_carried:1067793146467078255>" },
            { 9, "<:state_sylvan_m:1067808314085359756>" },
            { 10, "<:state_taming:1067808315230408745>" },
            { 11, "<:state_riding:1067808272955998290>" },
            { 12, "<:state_unruly:1067808319105933393>" },
            { 13, "<:state_ext_disob:1067793256093597796>" },
            { 14, "<:state_snowcover:1067808310474055701>" },
            { 15, "<:state_awaken:1067793141719121920>" },
            { 16, "<:state_vulnerable:1067808320448106617>" },
            { 17, "<:state_parted:1067793410901159956>" },
            { 18, "<:state_frozen:1067793307163443210>" },
            { 19, "<:state_cracked:1067793245863694336>" },
            { 27, "<:state_leopardo:1067793313672986634>" },
            { 28, "<:state_free:1067793305850630184>" },
            { 29, "<:state_odd_g:1067793407935778896>" },
            { 30, "<:state_even_g:1067793254873055372>" },
            { 31, "<:state_first_ink:1067793302365155348>" },
            { 32, "<:state_second_ink:1067808307571589190>" },
            { 33, "<:state_third_ink:1067808316492877834>" },
            { 34, "<:state_fourth_ink:1067793304428761139>" },
            { 35, "<:state_kill:1067793311605207050>" },
            { 36, "<:state_paralyze:1067793409147932672>" },
            { 37, "<:state_curse:1067793247210049598>" },
            { 38, "<:state_poison:1067808245328138240>" },
            { 39, "<:state_blurry:1067793142943854632>" },
            { 40, "<:state_corrupted:1067793243548426371>" },
            { 41, "<:state_silent:1067808309446443019>" },
            { 42, "<:state_weakened:1067808321710587945>" },
            { 48, "<:state_confused:1067793150237745172>" },
            { 49, "<:state_ghoulified:1067793308996345936>" },
            { 50, "<:state_altruistic:1067793139609387109>" },
            { 55, "<:state_retired:1067808270456213544>" },
            { 60, "<:state_devoted:1067793248833257614>" },
            { 61, "<:state_aggressive:1067793137961013288>" },
            { 73, "<:state_boggedown:1067793144801939497>" }
        };

        public static string State(int id)
        {
            return STATES.TryGetValue(id, out string? emoji) ? emoji : EMPTY;
        }


        //Runes
        public static readonly Dictionary<int, string> BA_RUNES = new()
        {
            { 1, "<:rune_fo:971137161153884221>" },
            { 2, "<:rune_sa:971137179432657017>" },
            { 3, "<:rune_ine:971137162097606666>" },
            { 4, "<:rune_vi:971137180191830116>" },
            { 5, "<:rune_age:971137127670747207>" },
            { 6, "<:rune_cha:971137156447883314>" },
            { 7, "<:rune_ga_pa:971129483673341992>" },
            { 8, "<:rune_ga_pme:971129485036490762>" },
            { 9, "<:rune_cri:971137156670189578>" },
            { 10, "<:rune_so:971137125443588146>" },
            { 11, "<:rune_do:971137156926042182>" },
            { 12, "<:rune_do_per:971137181127163905>" },
            { 13, "<:rune_do_ren:971137158121398292>" },
            { 14, "<:rune_po:971129485992804412>" },
            { 15, "<:rune_summo:971137179642368001>" },
            { 16, "<:rune_pod:971137173090885632>" },
            { 17, "<:rune_pi:971137172210069504>" },
            { 18, "<:rune_pi_per:971137186139365376>" },
            { 19, "<:rune_ini:971137162470895636>" },
            { 20, "<:rune_prospe:971137173216714752>" },
            { 21, "<:rune_fire_re:971137182263820318>" },
            { 22, "<:rune_air_re:971137166602301590>" },
            { 23, "<:rune_water_re:971137188450414723>" },
            { 24, "<:rune_earth_re:971137181890510958>" },
            { 25, "<:rune_neutral_re:971137183316578385>" },
            { 26, "<:rune_fire_re_per:971137182620323840>" },
            { 27, "<:rune_air_re_per:971137181638869042>" },
            { 28, "<:rune_earth_re_per:971137182003761162>" },
            { 29, "<:rune_neutral_re_per:971137183576629368>" },
            { 30, "<:rune_water_re_per:971137188899201124>" },
            { 31, "<:rune_hunting:971137189087944795>" }
        };

        public static string BaRune(int id)
        {
            return BA_RUNES.TryGetValue(id, out string? emoji) ? emoji : EMPTY;
        }

        public static readonly Dictionary<int, string> PA_RUNES = new()
        {
            { 1, "<:rune_pa_fo:971137184532922378>" },
            { 2, "<:rune_pa_sa:971137185392767089>" },
            { 3, "<:rune_pa_ine:971137184843300915>" },
            { 4, "<:rune_pa_vi:971137171480248392>" },
            { 5, "<:rune_pa_age:971137183798927430>" },
            { 6, "<:rune_pa_cha:971137183912190032>" },
            { 12, "<:rune_pa_do_per:971137189322838096>" },
            { 16, "<:rune_pa_pod:971137169680924692>" },
            { 17, "<:rune_pa_pi:971137185183072266>" },
            { 18, "<:rune_pa_pi_per:971137185610891294>" },
            { 19, "<:rune_pa_ini:971137167957032960>" },
            { 20, "<:rune_pa_prospe:971137170217775104>" }
        };

        public static string PaRune(int id)
        {
            return PA_RUNES.TryGetValue(id, out string? emoji) ? emoji : EMPTY;
        }

        public static readonly Dictionary<int, string> RA_RUNES = new()
        {
            { 1, "<:rune_ra_fo:971137186927898666>" },
            { 2, "<:rune_ra_sa:971137188081307688>" },
            { 3, "<:rune_ra_ine:971137187485720596>" },
            { 4, "<:rune_ra_vi:971137188060340254>" },
            { 5, "<:rune_ra_age:971137186072240240>" },
            { 6, "<:rune_ra_cha:971137186739134525>" },
            { 12, "<:rune_ra_do_per:971137189780025374>" },
            { 16, "<:rune_ra_pod:971137178006618162>" },
            { 18, "<:rune_ra_pi_per:971137187896766534>" },
            { 19, "<:rune_ra_ini:971137177071272006>" }
        };

        public static string RaRune(int id)
        {
            return RA_RUNES.TryGetValue(id, out string? emoji) ? emoji : EMPTY;
        }

        //Areas
        public static readonly Dictionary<int, string> AREAS = new()
        {
            { 1, "<:area_cross:1107731330999013456>" },
            { 2, "<:area_perpendicular_line:1107731337886056521>" },
            { 3, "<:area_circle:1107731328058806344>" },
            { 4, "<:area_dot:1107731332160823346>" },
            { 5, "<:area_line:1107731335910543382>" },
            { 6, "<:area_checked_patern:1110877139810652261>" },
            { 7, "<:area_ring:1107731517473562745>" },
            { 8, "<:area_square:1107731519428112495>" },
            { 9, "<:area_void_square:1107731523978932245>" },
            { 10, "<:area_cone:1107731329929449642>" },
            { 11, "<:area_diagonal_cross:1107744151098892400>" },
            { 12, "<:area_t:1107731520883535942>" },
            { 13, "<:area_t:1107731520883535942>" },
            { 15, "<:area_void_cross:1107731522905198613>" },
            { 16, "<:area_large_line:1107731334966820924>" },
            { 17, "<:area_point:1107731516320129135>" },
            { 21, "<:area_star:1107731341417660456>" },
            { 23, "<:area_fork:1107731333867909280>" },
            { 67, "<:area_circle:1107731328058806344>" },
            { 68, "<:area_checked_patern:1110877139810652261>" },
            { 76, "<:area_line:1107731335910543382>" },
            { 79, "<:area_ring:1107731517473562745>" },
            { 80, "<:area_dot:1107731332160823346>" },
            { 84, "<:area_perpendicular_line:1107731337886056521>" },
            { 88, "<:area_cross:1107731330999013456>" }
        };

        public static string Area(int id)
        {
            return AREAS.TryGetValue(id, out string? emoji) ? emoji : UNKNOWN;
        }


        //Bool
        public const string TRUE = "<:true:971147169908289556>";
        public const string FALSE = "<:false:971147169966997514>";

        public static string Bool(bool value)
        {
            return value ? TRUE : FALSE;
        }


        //Quests
        public const string QUEST = "<:quest:1095984758921625712>";
        public const string REPEATABLE_QUEST = "<:repeatable_quest:1095984761178165320>";
        public const string ACCOUNT_QUEST = "<:account_quest:1095984762482597888>";
        public const string ACCOUNT_REPEATABLE_QUEST = "<:account_repeatable_quest:1095984763812188201>";

        public static string Quest(bool isRepeatable, bool isAccount)
        {
            if (isRepeatable && isAccount)
            {
                return ACCOUNT_REPEATABLE_QUEST;
            }

            if (isAccount)
            {
                return ACCOUNT_QUEST;
            }

            if (isRepeatable)
            {
                return REPEATABLE_QUEST;
            }

            return QUEST;
        }


        //Others
        public const string EMPTY = "<:empty:971146087572316230>";
        public const string HOUSE = "<:house:971448610610884718>";
        public const string DUNGEON = "<:dungeon:971144890928996352>";
        public const string KAMAS = "<:kamas:971144891163885618>";
        public const string XP = "<:xp:971144890870300732>";
        public const string UNKNOWN = "<:unknown:1111058034941251695>";
    }
}
