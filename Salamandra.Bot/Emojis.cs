namespace Salamandra.Bot
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
        public const string STATE_ALTRUISTIC = "<:state_altruistic:976523161644441632>";
        public const string STATE_CORRUPTED = "<:state_corrupted:976523162235854878>";
        public const string STATE_DRUNK = "<:state_drunk:976523162177142795>";
        public const string STATE_GRAVITY = "<:state_gravity:976523162797867058>";
        public const string STATE_RETIRED = "<:state_retired:976523162672050217>";
        public const string STATE_ROOTED = "<:state_rooted:976523163267657748>";
        public const string STATE_SOUL_SEEKER = "<:state_soul_seeker:976523163498319974>";
        public const string STATE_TAMING = "<:state_taming:976523163812892692>";
        public const string STATE_WEAKENED = "<:state_weakened:976523164010033212>";

        public static string State(int id)
        {
            switch (id)
            {
                case 1:
                    return STATE_DRUNK;
                case 2:
                    return STATE_SOUL_SEEKER;
                case 6:
                    return STATE_ROOTED;
                case 7:
                    return STATE_GRAVITY;
                case 10:
                    return STATE_TAMING;
                case 40:
                    return STATE_CORRUPTED;
                case 42:
                    return STATE_WEAKENED;
                case 50:
                    return STATE_ALTRUISTIC;
                case 55:
                    return STATE_RETIRED;
                default:
                    return EMPTY;
            }
        }


        //Runes
        public const string RUNE_FO = "<:rune_fo:971137161153884221>";
        public const string RUNE_PA_FO = "<:rune_pa_fo:971137184532922378>";
        public const string RUNE_RA_FO = "<:rune_ra_fo:971137186927898666>";
        public const string RUNE_SA = "<:rune_sa:971137179432657017>";
        public const string RUNE_PA_SA = "<:rune_pa_sa:971137185392767089>";
        public const string RUNE_RA_SA = "<:rune_ra_sa:971137188081307688>";
        public const string RUNE_INE = "<:rune_ine:971137162097606666>";
        public const string RUNE_PA_INE = "<:rune_pa_ine:971137184843300915>";
        public const string RUNE_RA_INE = "<:rune_ra_ine:971137187485720596>";
        public const string RUNE_VI = "<:rune_vi:971137180191830116>";
        public const string RUNE_PA_VI = "<:rune_pa_vi:971137171480248392>";
        public const string RUNE_RA_VI = "<:rune_ra_vi:971137188060340254>";
        public const string RUNE_AGE = "<:rune_age:971137127670747207>";
        public const string RUNE_PA_AGE = "<:rune_pa_age:971137183798927430>";
        public const string RUNE_RA_AGE = "<:rune_ra_age:971137186072240240>";
        public const string RUNE_CHA = "<:rune_cha:971137156447883314>";
        public const string RUNE_PA_CHA = "<:rune_pa_cha:971137183912190032>";
        public const string RUNE_RA_CHA = "<:rune_ra_cha:971137186739134525>";
        public const string RUNE_CRI = "<:rune_cri:971137156670189578>";
        public const string RUNE_SO = "<:rune_so:971137125443588146>";
        public const string RUNE_DO = "<:rune_do:971137156926042182>";
        public const string RUNE_DO_PER = "<:rune_do_per:971137181127163905>";
        public const string RUNE_PA_DO_PER = "<:rune_pa_do_per:971137189322838096>";
        public const string RUNE_RA_DO_PER = "<:rune_ra_do_per:971137189780025374>";
        public const string RUNE_DO_REN = "<:rune_do_ren:971137158121398292>";
        public const string RUNE_SUMMO = "<:rune_summo:971137179642368001>";
        public const string RUNE_POD = "<:rune_pod:971137173090885632>";
        public const string RUNE_PA_POD = "<:rune_pa_pod:971137169680924692>";
        public const string RUNE_RA_POD = "<:rune_ra_pod:971137178006618162>";
        public const string RUNE_PI = "<:rune_pi:971137172210069504>";
        public const string RUNE_PA_PI = "<:rune_pa_pi:971137185183072266>";
        public const string RUNE_PI_PER = "<:rune_pi_per:971137186139365376>";
        public const string RUNE_PA_PI_PER = "<:rune_pa_pi_per:971137185610891294>";
        public const string RUNE_RA_PI_PER = "<:rune_ra_pi_per:971137187896766534>";
        public const string RUNE_INI = "<:rune_ini:971137162470895636>";
        public const string RUNE_PA_INI = "<:rune_pa_ini:971137167957032960>";
        public const string RUNE_RA_INI = "<:rune_ra_ini:971137177071272006>";
        public const string RUNE_PROSPE = "<:rune_prospe:971137173216714752>";
        public const string RUNE_PA_PROSPE = "<:rune_pa_prospe:971137170217775104>";
        public const string RUNE_FIRE_RE = "<:rune_fire_re:971137182263820318>";
        public const string RUNE_AIR_RE = "<:rune_air_re:971137166602301590>";
        public const string RUNE_WATER_RE = "<:rune_water_re:971137188450414723>";
        public const string RUNE_EARTH_RE = "<:rune_earth_re:971137181890510958>";
        public const string RUNE_NEUTRAL_RE = "<:rune_neutral_re:971137183316578385>";
        public const string RUNE_FIRE_RE_PER = "<:rune_fire_re_per:971137182620323840>";
        public const string RUNE_AIR_RE_PER = "<:rune_air_re_per:971137181638869042>";
        public const string RUNE_EARTH_RE_PER = "<:rune_earth_re_per:971137182003761162>";
        public const string RUNE_NEUTRAL_RE_PER = "<:rune_neutral_re_per:971137183576629368>";
        public const string RUNE_WATER_RE_PER = "<:rune_water_re_per:971137188899201124>";
        public const string RUNE_HUNTING = "<:rune_hunting:971137189087944795>";
        public const string RUNE_PO = "<:rune_po:971129485992804412>";
        public const string RUNE_GA_PA = "<:rune_ga_pa:971129483673341992>";
        public const string RUNE_GA_PME = "<:rune_ga_pme:971129485036490762>";

        public static string BaRune(int id)
        {
            switch (id)
            {
                case 1:
                    return RUNE_FO;
                case 2:
                    return RUNE_SA;
                case 3:
                    return RUNE_INE;
                case 4:
                    return RUNE_VI;
                case 5:
                    return RUNE_AGE;
                case 6:
                    return RUNE_CHA;
                case 7:
                    return RUNE_GA_PA;
                case 8:
                    return RUNE_GA_PME;
                case 9:
                    return RUNE_CRI;
                case 10:
                    return RUNE_SO;
                case 11:
                    return RUNE_DO;
                case 12:
                    return RUNE_DO_PER;
                case 13:
                    return RUNE_DO_REN;
                case 14:
                    return RUNE_PO;
                case 15:
                    return RUNE_SUMMO;
                case 16:
                    return RUNE_POD;
                case 17:
                    return RUNE_PI;
                case 18:
                    return RUNE_PI_PER;
                case 19:
                    return RUNE_INI;
                case 20:
                    return RUNE_PROSPE;
                case 21:
                    return RUNE_FIRE_RE;
                case 22:
                    return RUNE_AIR_RE;
                case 23:
                    return RUNE_WATER_RE;
                case 24:
                    return RUNE_EARTH_RE;
                case 25:
                    return RUNE_NEUTRAL_RE;
                case 26:
                    return RUNE_FIRE_RE_PER;
                case 27:
                    return RUNE_AIR_RE_PER;
                case 28:
                    return RUNE_EARTH_RE_PER;
                case 29:
                    return RUNE_NEUTRAL_RE_PER;
                case 30:
                    return RUNE_WATER_RE_PER;
                case 31:
                    return RUNE_HUNTING;
                default:
                    return EMPTY;
            }
        }

        public static string PaRune(int id)
        {
            switch (id)
            {
                case 1:
                    return RUNE_PA_FO;
                case 2:
                    return RUNE_PA_SA;
                case 3:
                    return RUNE_PA_INE;
                case 4:
                    return RUNE_PA_VI;
                case 5:
                    return RUNE_PA_AGE;
                case 6:
                    return RUNE_PA_CHA;
                case 12:
                    return RUNE_PA_DO_PER;
                case 16:
                    return RUNE_PA_POD;
                case 17:
                    return RUNE_PA_PI;
                case 18:
                    return RUNE_PA_PI_PER;
                case 19:
                    return RUNE_PA_INI;
                case 20:
                    return RUNE_PA_PROSPE;
                default:
                    return EMPTY;
            }
        }

        public static string RaRune(int id)
        {
            switch (id)
            {
                case 1:
                    return RUNE_RA_FO;
                case 2:
                    return RUNE_RA_SA;
                case 3:
                    return RUNE_RA_INE;
                case 4:
                    return RUNE_RA_VI;
                case 5:
                    return RUNE_RA_AGE;
                case 6:
                    return RUNE_RA_CHA;
                case 12:
                    return RUNE_RA_DO_PER;
                case 16:
                    return RUNE_RA_POD;
                case 18:
                    return RUNE_RA_PI_PER;
                case 19:
                    return RUNE_RA_INI;
                default:
                    return EMPTY;
            }
        }


        //Areas
        public const string AREA_CIRCLE = "<:area_circle:971129517085184020>";
        public const string AREA_LINE = "<:area_line:971129517760475196>";
        public const string AREA_RING = "<:area_ring:971129517919846511>";
        public const string AREA_T = "<:area_t:971129518439944302>";
        public const string AREA_CROSS = "<:area_cross:971129517517197322>";
        public const string AREA_CHECKED_PATTERN = "<:area_checked_pattern:971129516728659978>";

        public static string Area(char symbol)
        {
            switch (symbol)
            {
                case 'C':
                    return AREA_CIRCLE;
                case 'L':
                    return AREA_LINE;
                case 'O':
                    return AREA_RING;
                case 'T':
                    return AREA_T;
                case 'X':
                    return AREA_CROSS;
                case 'D':
                    return AREA_CHECKED_PATTERN;
                default:
                    return EMPTY;
            }
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
