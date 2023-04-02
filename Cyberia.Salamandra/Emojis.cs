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
            switch (id)
            {
                case 226:
                    return TRAP_PERCENT;
                case 225:
                    return TRAP_DAMAGE;
                case 724:
                    return TITLE;
                case 182:
                case 780:
                    return SUMMONABLE_CREATURES;
                case 118:
                case 157:
                case 271:
                case 607:
                    return STRENGHT;
                case 130:
                    return STEAL_KAMAS;
                case 283:
                case 293:
                    return SPELL_DAMMAGE;
                case 289:
                    return SIGHT;
                case 106:
                case 220:
                    return RETURN;
                case 211:
                case 216:
                case 241:
                case 246:
                case 251:
                case 256:
                case 261:
                    return RES_WATER;
                case 214:
                case 219:
                case 244:
                case 249:
                case 254:
                case 259:
                case 264:
                    return RES_NEUTRAL;
                case 213:
                case 218:
                case 243:
                case 248:
                case 253:
                case 258:
                case 263:
                    return RES_FIRE;
                case 210:
                case 215:
                case 240:
                case 245:
                case 250:
                case 255:
                case 260:
                    return RES_EARTH;
                case 212:
                case 217:
                case 242:
                case 247:
                case 252:
                case 257:
                case 262:
                    return RES_AIR;
                case 116:
                case 117:
                case 135:
                case 136:
                case 320:
                    return RANGE;
                case 5:
                case 6:
                case 783:
                    return PUSH;
                case 176:
                case 177:
                    return PROSPECTING;
                case 158:
                case 159:
                    return WEIGHT;
                case 89:
                case 95:
                case 100:
                case 279:
                case 670:
                case 671:
                case 672:
                    return NEUTRAL_DAMAGE;
                case 77:
                case 78:
                case 127:
                case 128:
                case 134:
                case 169:
                    return MOVEMENT_POINTS;
                case 123:
                case 152:
                case 266:
                case 608:
                    return LUCK;
                case 146:
                    return LANGUAGE;
                case 126:
                case 155:
                case 269:
                case 611:
                    return INTELLIGENCE;
                case 669:
                    return INCARNATION;
                case 174:
                case 175:
                    return INITIATIVE;
                case 795:
                    return HUNTING;
                case 110:
                case 800:
                    return HEALTH;
                case 81:
                case 108:
                case 143:
                case 178:
                case 179:
                case 284:
                case 646:
                    return HEAL;
                case 148:
                    return FOLLOWER;
                case 88:
                case 94:
                case 99:
                case 278:
                    return FIRE_DAMAGE;
                case 281:
                case 294:
                    return ENHANCE_RANGE;
                case 139:
                case 230:
                    return ENERGY;
                case 282:
                    return ENABLE_RANGE;
                case 10:
                    return EMOTE;
                case 86:
                case 92:
                case 97:
                case 276:
                    return EARTH_DAMAGE;
                case 161:
                    return DODGE_MP;
                case 160:
                    return DODGE_AP;
                case 138:
                case 186:
                    return DAMAGES_PERCENT;
                case 112:
                case 121:
                case 145:
                    return DAMAGE;
                case 115:
                case 171:
                case 287:
                    return CRIT;
                case 286:
                    return CAST_SPEED;
                case 290:
                    return CAST_PER_TURN;
                case 285:
                    return AP_COST_REDUCTION;
                case 87:
                case 93:
                case 98:
                case 277:
                    return AIR_DAMAGE;
                case 119:
                case 154:
                case 268:
                case 609:
                    return AGILITY;
                case 84:
                case 101:
                case 111:
                case 120:
                case 133:
                case 168:
                case 2100:
                    return ACTION_POINTS;
                case 124:
                case 156:
                case 270:
                case 606:
                    return WISDOM;
                case 165:
                    return WEAPON_DAMAGE;
                case 85:
                case 91:
                case 96:
                case 275:
                    return WATER_DAMAGE;
                case 125:
                case 153:
                case 267:
                case 610:
                    return VITALITY;
                case 141:
                    return DEAD;
                case 163:
                    return ATTACK_MP;
                case 162:
                case 166:
                    return ATTACK_AP;
                default:
                    return EMPTY;
            }
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
        public static readonly Dictionary<char, string> AREAS = new()
        {
            { 'C', "<:area_circle:971129517085184020>" },
            { 'L', "<:area_line:971129517760475196>" },
            { 'O', "<:area_ring:971129517919846511>" },
            { 'T', "<:area_t:971129518439944302>" },
            { 'X', "<:area_cross:971129517517197322>" },
            { 'D', "<:area_checked_pattern:971129516728659978>" }
        };

        public static string Area(char symbol)
        {
            return AREAS.TryGetValue(symbol, out string? emoji) ? emoji : EMPTY;
        }


        //Bool
        public const string TRUE = "<:true:971147169908289556>";
        public const string FALSE = "<:false:971147169966997514>";

        public static string Bool(bool value)
        {
            return value ? TRUE : FALSE;
        }


        //Others
        public const string EMPTY = "<:empty:971146087572316230>";
        public const string HOUSE = "<:house:971448610610884718>";
        public const string DUNGEON = "<:dungeon:971144890928996352>";
        public const string QUEST = "<:quest:971144890622828615>";
        public const string QUEST_REPEATABLE = "<:quest_repeatable:971144890983538698>";
        public const string KAMAS = "<:kamas:971144891163885618>";
        public const string XP = "<:xp:971144890870300732>";
    }
}
