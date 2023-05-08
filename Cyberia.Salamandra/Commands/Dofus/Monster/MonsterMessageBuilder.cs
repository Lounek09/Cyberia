using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class MonsterMessageBuilder : CustomMessageBuilder
    {
        private readonly Monster _monster;
        private readonly MonsterRace? _monsterRace;
        private readonly MonsterSuperRace? _monsterSuperRace;
        private MonsterGrade? _currentMonsterGrade;
        private int _grade;

        public MonsterMessageBuilder(Monster monster) :
            base()
        {
            _monster = monster;
            _monsterRace = monster.GetRace();
            _monsterSuperRace = _monsterRace?.GetMonsterSuperRace();
            _currentMonsterGrade = monster.GetGrade(1);
            _grade = 1;
        }

        protected override async Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Bestiary, "Bestiaire")
                .WithTitle($"{_monster.Name} ({_monster.Id}) - Grade " + _grade)
                .WithThumbnail(await _monster.GetImgPath());

            if (_monsterSuperRace is not null)
                embed.AddField("Ecosystème :", _monsterSuperRace.Name, true);

            if (_monsterRace is not null)
                embed.AddField("Race :", _monsterRace.Name, true);

            if (_currentMonsterGrade is not null)
            {
                string alignmentName = _monster.GetAlignementName();
                if (!string.IsNullOrEmpty(alignmentName))
                    embed.AddField("Alignement :", alignmentName, true);


                string caracteristics = $"Niveau. {Formatter.Bold(_currentMonsterGrade.Level.ToString())}\n";
                caracteristics += _currentMonsterGrade.LifePoint is not null ? $"{Emojis.HEALTH}{Formatter.Bold(_currentMonsterGrade.LifePoint.Value.ToStringThousandSeparator())} " : "";
                caracteristics += _currentMonsterGrade.ActionPoint is not null ? $"{Emojis.ACTION_POINTS}{Formatter.Bold(_currentMonsterGrade.ActionPoint.Value.ToStringThousandSeparator())} " : "";
                caracteristics += _currentMonsterGrade.MovementPoint is not null ? $"{Emojis.MOVEMENT_POINTS}{Formatter.Bold(_currentMonsterGrade.MovementPoint.Value.ToStringThousandSeparator())} " : "";
                caracteristics += $"{Emojis.DODGE_AP}{Formatter.Bold(_currentMonsterGrade.GetActionPointDodge().ToStringThousandSeparator())}% ";
                caracteristics += $"{Emojis.DODGE_MP}{Formatter.Bold(_currentMonsterGrade.GetMovementPointDodge().ToStringThousandSeparator())}%\n";
                caracteristics += $"{Emojis.RES_NEUTRAL}{Formatter.Bold(_currentMonsterGrade.GetNeutralResistance().ToStringThousandSeparator())}% ";
                caracteristics += $"{Emojis.RES_EARTH}{Formatter.Bold(_currentMonsterGrade.GetEarthResistance().ToStringThousandSeparator())}% ";
                caracteristics += $"{Emojis.RES_FIRE}{Formatter.Bold(_currentMonsterGrade.GetFireResistance().ToStringThousandSeparator())}% ";
                caracteristics += $"{Emojis.RES_WATER}{Formatter.Bold(_currentMonsterGrade.GetWaterResistance().ToStringThousandSeparator())}% ";
                caracteristics += $"{Emojis.RES_AIR}{Formatter.Bold(_currentMonsterGrade.GetAirResistance().ToStringThousandSeparator())}%";
                embed.AddField("Caractéristiques :", caracteristics);

                if (!string.IsNullOrEmpty(_monster.TrelloUrl))
                    embed.AddField(Constant.ZERO_WIDTH_SPACE, _monster.TrelloUrl);
            }

            return embed;
        }

        private HashSet<DiscordButtonComponent> GradeButtons1Builder()
        {
            HashSet<DiscordButtonComponent> components = new();

            for (int i = 1; i < 6; i++)
            {
                if (_monster.GetGrade(i) is not null)
                    components.Add(new(ButtonStyle.Primary, i.ToString(), i.ToString(), _grade == i));
            }

            return components;
        }

        private HashSet<DiscordButtonComponent> GradeButtons2Builder()
        {
            HashSet<DiscordButtonComponent> components = new();

            for (int i = 6; i < 11; i++)
            {
                if (_monster.GetGrade(i) is not null)
                    components.Add(new(ButtonStyle.Primary, i.ToString(), i.ToString(), _grade == i));
            }

            return components;
        }

        protected override async Task<DiscordInteractionResponseBuilder> InteractionResponseBuilder()
        {
            DiscordInteractionResponseBuilder response = await base.InteractionResponseBuilder();

            HashSet<DiscordButtonComponent> gradeButtons = GradeButtons1Builder();
            if (gradeButtons.Count > 0)
                response.AddComponents(gradeButtons);

            gradeButtons = GradeButtons2Builder();
            if (gradeButtons.Count > 0)
                response.AddComponents(gradeButtons);

            return response;
        }

        protected override async Task<DiscordFollowupMessageBuilder> FollowupMessageBuilder()
        {
            DiscordFollowupMessageBuilder followupMessage = await base.FollowupMessageBuilder();

            HashSet<DiscordButtonComponent> gradeButtons = GradeButtons1Builder();
            if (gradeButtons.Count > 0)
                followupMessage.AddComponents(gradeButtons);

            gradeButtons = GradeButtons2Builder();
            if (gradeButtons.Count > 0)
                followupMessage.AddComponents(gradeButtons);

            return followupMessage;
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (int.TryParse(e.Id, out int grade))
            {
                _grade = grade;
                _currentMonsterGrade = _monster.GetGrade(grade);

                await UpdateInteractionResponse(e.Interaction);
                return true;
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
