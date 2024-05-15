using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Api.Data.Alignments;
using Cyberia.Api.Data.Monsters;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus.Monster;

public sealed class MonsterMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "M";
    public const int PacketVersion = 1;

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
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var monsterId) &&
            int.TryParse(parameters[1], out var selectedGrade))
        {
            var monsterData = DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(monsterId);
            if (monsterData is not null)
            {
                return new MonsterMessageBuilder(monsterData, selectedGrade);
            }
        }

        return null;
    }

    public static string GetPacket(int monsterId, int selectedGrade = 1)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, monsterId, selectedGrade);
    }

    public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        var buttons = GradeButtons1Builder();
        if (buttons.Any())
        {
            message.AddComponents(buttons);
        }

        buttons = GradeButtons2Builder();
        if (buttons.Any())
        {
            message.AddComponents(buttons);
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Bestiary, "Bestiaire")
            .WithTitle($"{_monsterData.Name} ({_monsterData.Id}) - Grade " + _selectedGrade)
            .WithThumbnail(await _monsterData.GetBigImagePathAsync(CdnImageSize.Size128));

        if (_monsterSuperRaceData is not null)
        {
            embed.AddField("Ecosystème :", _monsterSuperRaceData.Name, true);
        }

        if (_monsterRaceData is not null)
        {
            embed.AddField("Race :", _monsterRaceData.Name, true);
        }

        if (_monsterGradeData is not null)
        {
            if (_alignmentData is not null)
            {
                embed.AddField("Alignement :", _alignmentData.Name, true);
            }

            StringBuilder caracBuilder = new();

            caracBuilder.Append("Niveau. ");
            caracBuilder.Append(Formatter.Bold(_monsterGradeData.Level.ToString()));
            caracBuilder.Append('\n');

            if (_monsterGradeData.LifePoint is not null)
            {
                caracBuilder.Append(Emojis.EffectHealthPoint);
                caracBuilder.Append(Formatter.Bold(_monsterGradeData.LifePoint.Value.ToFormattedString()));
                caracBuilder.Append(' ');
            }

            if (_monsterGradeData.ActionPoint is not null)
            {
                caracBuilder.Append(Emojis.EffectAp);
                caracBuilder.Append(Formatter.Bold(_monsterGradeData.ActionPoint.Value.ToFormattedString()));
                caracBuilder.Append(' ');
            }

            if (_monsterGradeData.MovementPoint is not null)
            {
                caracBuilder.Append(Emojis.EffectMp);
                caracBuilder.Append(Formatter.Bold(_monsterGradeData.MovementPoint.Value.ToFormattedString()));
                caracBuilder.Append(' ');
            }

            caracBuilder.Append(Emojis.EffectApResistance);
            caracBuilder.Append(Formatter.Bold(_monsterGradeData.GetActionPointDodge().ToFormattedString()));
            caracBuilder.Append("% ");

            caracBuilder.Append(Emojis.EffectMpResistance);
            caracBuilder.Append(Formatter.Bold(_monsterGradeData.GetMovementPointDodge().ToFormattedString()));
            caracBuilder.Append("%\n");

            caracBuilder.Append(Emojis.EffectNeutralResistance);
            caracBuilder.Append(Formatter.Bold(_monsterGradeData.GetNeutralResistance().ToFormattedString()));
            caracBuilder.Append("% ");

            caracBuilder.Append(Emojis.EffectEarthResistance);
            caracBuilder.Append(Formatter.Bold(_monsterGradeData.GetEarthResistance().ToFormattedString()));
            caracBuilder.Append("% ");

            caracBuilder.Append(Emojis.EffectFireResistance);
            caracBuilder.Append(Formatter.Bold(_monsterGradeData.GetFireResistance().ToFormattedString()));
            caracBuilder.Append("% ");

            caracBuilder.Append(Emojis.EffectWaterResistance);
            caracBuilder.Append(Formatter.Bold(_monsterGradeData.GetWaterResistance().ToFormattedString()));
            caracBuilder.Append("% ");

            caracBuilder.Append(Emojis.EffectAirResistance);
            caracBuilder.Append(Formatter.Bold(_monsterGradeData.GetAirResistance().ToFormattedString()));
            caracBuilder.Append('%');

            embed.AddField("Caractéristiques :", caracBuilder.ToString());

            if (!string.IsNullOrEmpty(_monsterData.TrelloUrl))
            {
                embed.AddField(Constant.ZeroWidthSpace, _monsterData.TrelloUrl);
            }
        }

        return embed;
    }

    private IEnumerable<DiscordButtonComponent> GradeButtons1Builder()
    {
        for (var i = 0; i < Constant.MaxButtonPerRow; i++)
        {
            var y = i + 1;
            if (_monsterData.GetMonsterGradeData(y) is not null)
            {
                yield return new(DiscordButtonStyle.Primary, GetPacket(_monsterData.Id,y), y.ToString(), _selectedGrade == y);
            }
        }
    }

    private IEnumerable<DiscordButtonComponent> GradeButtons2Builder()
    {
        for (var i = Constant.MaxButtonPerRow; i < Constant.MaxButtonPerRow * 2; i++)
        {
            var y = i + 1;
            if (_monsterData.GetMonsterGradeData(y) is not null)
            {
                yield return new(DiscordButtonStyle.Primary, GetPacket(_monsterData.Id, y), y.ToString(), _selectedGrade == y);
            }
        }
    }
}
