using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class MonsterMessageBuilder : ICustomMessageBuilder
    {
        public const string PACKET_HEADER = "M";
        public const int PACKET_VERSION = 1;

        private readonly Monster _monster;
        private readonly int _selectedGrade;
        private readonly MonsterGrade? _monsterGrade;
        private readonly MonsterRace? _monsterRace;
        private readonly MonsterSuperRace? _monsterSuperRace;

        public MonsterMessageBuilder(Monster monster, int selectedGrade = 1)
        {
            _monster = monster;
            _selectedGrade = selectedGrade;
            _monsterGrade = _monster.GetGrade(selectedGrade);
            _monsterRace = monster.GetRace();
            _monsterSuperRace = _monsterRace?.GetMonsterSuperRace();
        }

        public static MonsterMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 1 &&
                int.TryParse(parameters[0], out int monsterId) &&
                int.TryParse(parameters[1], out int selectedGrade))
            {
                Monster? monster = Bot.Instance.Api.Datacenter.MonstersData.GetMonsterById(monsterId);
                if (monster is not null)
                    return new MonsterMessageBuilder(monster, selectedGrade);
            }

            return null;
        }

        public static string GetPacket(int monsterId, int selectedGrade = 1)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, monsterId, selectedGrade);
        }

        public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
        {
            IDiscordMessageBuilder message = new T()
                .AddEmbed(await EmbedBuilder());

            List<DiscordButtonComponent> buttons = Buttons1Builder();
            if (buttons.Count > 0)
                message.AddComponents(buttons);

            buttons = Buttons2Builder();
            if (buttons.Count > 0)
                message.AddComponents(buttons);

            return (T)message;
        }

        private async Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Bestiary, "Bestiaire")
                .WithTitle($"{_monster.Name} ({_monster.Id}) - Grade " + _selectedGrade)
                .WithThumbnail(await _monster.GetImagePath());

            if (_monsterSuperRace is not null)
                embed.AddField("Ecosystème :", _monsterSuperRace.Name, true);

            if (_monsterRace is not null)
                embed.AddField("Race :", _monsterRace.Name, true);

            if (_monsterGrade is not null)
            {
                string alignmentName = _monster.GetAlignementName();
                if (!string.IsNullOrEmpty(alignmentName))
                    embed.AddField("Alignement :", alignmentName, true);

                StringBuilder caracteristicsBuilder = new();
                caracteristicsBuilder.AppendFormat("Niveau. {0}\n", Formatter.Bold(_monsterGrade.Level.ToString()));
                if (_monsterGrade.LifePoint is not null)
                    caracteristicsBuilder.AppendFormat("{0}{1} ", Emojis.HEALTH, Formatter.Bold(_monsterGrade.LifePoint.Value.ToStringThousandSeparator()));
                if (_monsterGrade.ActionPoint is not null)
                    caracteristicsBuilder.AppendFormat("{0}{1} ", Emojis.ACTION_POINTS, Formatter.Bold(_monsterGrade.ActionPoint.Value.ToStringThousandSeparator()));
                if (_monsterGrade.MovementPoint is not null)
                    caracteristicsBuilder.AppendFormat("{0}{1} ", Emojis.MOVEMENT_POINTS, Formatter.Bold(_monsterGrade.MovementPoint.Value.ToStringThousandSeparator()));
                caracteristicsBuilder.AppendFormat("{0}{1}% ", Emojis.DODGE_AP, Formatter.Bold(_monsterGrade.GetActionPointDodge().ToStringThousandSeparator()));
                caracteristicsBuilder.AppendFormat("{0}{1}%\n", Emojis.DODGE_MP, Formatter.Bold(_monsterGrade.GetMovementPointDodge().ToStringThousandSeparator()));
                caracteristicsBuilder.AppendFormat("{0}{1}% ", Emojis.RES_NEUTRAL, Formatter.Bold(_monsterGrade.GetNeutralResistance().ToStringThousandSeparator()));
                caracteristicsBuilder.AppendFormat("{0}{1}% ", Emojis.RES_EARTH, Formatter.Bold(_monsterGrade.GetEarthResistance().ToStringThousandSeparator()));
                caracteristicsBuilder.AppendFormat("{0}{1}% ", Emojis.RES_FIRE, Formatter.Bold(_monsterGrade.GetFireResistance().ToStringThousandSeparator()));
                caracteristicsBuilder.AppendFormat("{0}{1}% ", Emojis.RES_WATER, Formatter.Bold(_monsterGrade.GetWaterResistance().ToStringThousandSeparator()));
                caracteristicsBuilder.AppendFormat("{0}{1}%", Emojis.RES_AIR, Formatter.Bold(_monsterGrade.GetAirResistance().ToStringThousandSeparator()));
                embed.AddField("Caractéristiques :", caracteristicsBuilder.ToString());

                if (!string.IsNullOrEmpty(_monster.TrelloUrl))
                    embed.AddField(Constant.ZERO_WIDTH_SPACE, _monster.TrelloUrl);
            }

            return embed;
        }

        private List<DiscordButtonComponent> Buttons1Builder()
        {
            List<DiscordButtonComponent> components = new();

            for (int i = 1; i < 6; i++)
            {
                if (_monster.GetGrade(i) is not null)
                    components.Add(new(ButtonStyle.Primary, GetPacket(_monster.Id, i), i.ToString(), _selectedGrade == i));
            }

            return components;
        }

        private List<DiscordButtonComponent> Buttons2Builder()
        {
            List<DiscordButtonComponent> components = new();

            for (int i = 6; i < 11; i++)
            {
                if (_monster.GetGrade(i) is not null)
                    components.Add(new(ButtonStyle.Primary, GetPacket(_monster.Id, i), i.ToString(), _selectedGrade == i));
            }

            return components;
        }
    }
}
