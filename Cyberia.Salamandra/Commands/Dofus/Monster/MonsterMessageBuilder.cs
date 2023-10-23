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

        private readonly MonsterData _monsterData;
        private readonly int _selectedGrade;
        private readonly MonsterGradeData? _monsterGradeData;
        private readonly MonsterRaceData? _monsterRaceData;
        private readonly MonsterSuperRaceData? _monsterSuperRaceData;
        private readonly AlignmentData? _alignmentData;

        public MonsterMessageBuilder(MonsterData monster, int selectedGrade = 1)
        {
            _monsterData = monster;
            _selectedGrade = selectedGrade;
            _monsterGradeData = _monsterData.GetMonsterGradeData(selectedGrade);
            _monsterRaceData = monster.GetMonsterRaceData();
            _monsterSuperRaceData = _monsterRaceData?.GetMonsterSuperRaceData();
            _alignmentData = _monsterData.GetAlignmentData();
        }

        public static MonsterMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 1 &&
                int.TryParse(parameters[0], out int monsterId) &&
                int.TryParse(parameters[1], out int selectedGrade))
            {
                MonsterData? monsterData = Bot.Instance.Api.Datacenter.MonstersData.GetMonsterDataById(monsterId);
                if (monsterData is not null)
                    return new MonsterMessageBuilder(monsterData, selectedGrade);
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
                .WithTitle($"{_monsterData.Name} ({_monsterData.Id}) - Grade " + _selectedGrade)
                .WithThumbnail(await _monsterData.GetImagePath());

            if (_monsterSuperRaceData is not null)
                embed.AddField("Ecosystème :", _monsterSuperRaceData.Name, true);

            if (_monsterRaceData is not null)
                embed.AddField("Race :", _monsterRaceData.Name, true);

            if (_monsterGradeData is not null)
            {
                if (_alignmentData is not null)
                    embed.AddField("Alignement :", _alignmentData.Name, true);

                StringBuilder caracteristicsBuilder = new();
                caracteristicsBuilder.AppendFormat("Niveau. {0}\n", Formatter.Bold(_monsterGradeData.Level.ToString()));
                if (_monsterGradeData.LifePoint is not null)
                    caracteristicsBuilder.AppendFormat("{0}{1} ", Emojis.HEALTH, Formatter.Bold(_monsterGradeData.LifePoint.Value.ToStringThousandSeparator()));
                if (_monsterGradeData.ActionPoint is not null)
                    caracteristicsBuilder.AppendFormat("{0}{1} ", Emojis.ACTION_POINTS, Formatter.Bold(_monsterGradeData.ActionPoint.Value.ToStringThousandSeparator()));
                if (_monsterGradeData.MovementPoint is not null)
                    caracteristicsBuilder.AppendFormat("{0}{1} ", Emojis.MOVEMENT_POINTS, Formatter.Bold(_monsterGradeData.MovementPoint.Value.ToStringThousandSeparator()));
                caracteristicsBuilder.AppendFormat("{0}{1}% ", Emojis.DODGE_AP, Formatter.Bold(_monsterGradeData.GetActionPointDodge().ToStringThousandSeparator()));
                caracteristicsBuilder.AppendFormat("{0}{1}%\n", Emojis.DODGE_MP, Formatter.Bold(_monsterGradeData.GetMovementPointDodge().ToStringThousandSeparator()));
                caracteristicsBuilder.AppendFormat("{0}{1}% ", Emojis.RES_NEUTRAL, Formatter.Bold(_monsterGradeData.GetNeutralResistance().ToStringThousandSeparator()));
                caracteristicsBuilder.AppendFormat("{0}{1}% ", Emojis.RES_EARTH, Formatter.Bold(_monsterGradeData.GetEarthResistance().ToStringThousandSeparator()));
                caracteristicsBuilder.AppendFormat("{0}{1}% ", Emojis.RES_FIRE, Formatter.Bold(_monsterGradeData.GetFireResistance().ToStringThousandSeparator()));
                caracteristicsBuilder.AppendFormat("{0}{1}% ", Emojis.RES_WATER, Formatter.Bold(_monsterGradeData.GetWaterResistance().ToStringThousandSeparator()));
                caracteristicsBuilder.AppendFormat("{0}{1}%", Emojis.RES_AIR, Formatter.Bold(_monsterGradeData.GetAirResistance().ToStringThousandSeparator()));
                embed.AddField("Caractéristiques :", caracteristicsBuilder.ToString());

                if (!string.IsNullOrEmpty(_monsterData.TrelloUrl))
                    embed.AddField(Constant.ZERO_WIDTH_SPACE, _monsterData.TrelloUrl);
            }

            return embed;
        }

        private List<DiscordButtonComponent> Buttons1Builder()
        {
            List<DiscordButtonComponent> components = new();

            for (int i = 1; i < 6; i++)
            {
                if (_monsterData.GetMonsterGradeData(i) is not null)
                    components.Add(new(ButtonStyle.Primary, GetPacket(_monsterData.Id, i), i.ToString(), _selectedGrade == i));
            }

            return components;
        }

        private List<DiscordButtonComponent> Buttons2Builder()
        {
            List<DiscordButtonComponent> components = new();

            for (int i = 6; i < 11; i++)
            {
                if (_monsterData.GetMonsterGradeData(i) is not null)
                    components.Add(new(ButtonStyle.Primary, GetPacket(_monsterData.Id, i), i.ToString(), _selectedGrade == i));
            }

            return components;
        }
    }
}
