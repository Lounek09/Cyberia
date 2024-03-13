using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class LangsMessageBuilder : ICustomMessageBuilder
{
    public const string PACKET_HEADER = "LANG";
    public const int PACKET_VERSION = 1;

    private readonly LangType _type;
    private readonly LangLanguage _language;
    private readonly LangDataCollection _langDataCollection;

    public LangsMessageBuilder(LangType type, LangLanguage language)
    {
        _type = type;
        _language = language;
        _langDataCollection = LangsWatcher.Langs[(type, language)];
    }

    public static LangsMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PACKET_VERSION &&
            parameters.Length > 1 &&
            Enum.TryParse(parameters[0], out LangType langType) &&
            Enum.TryParse(parameters[1], out LangLanguage language))
        {
            return new LangsMessageBuilder(langType, language);
        }

        return null;
    }

    public static string GetPacket(LangType langType, LangLanguage language)
    {
        return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, langType, language);
    }

    public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        var select = Select1Builder();
        if (select.Options.Count > 0)
        {
            message.AddComponents(select);
        }

        var select2 = Select2Builder();
        if (select2.Options.Count > 0)
        {
            message.AddComponents(select2);
        }

        return (T)message;
    }

    private Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Langs")
            .WithTitle($"Langs {_type} en {_language}");

        if (_langDataCollection.Count > 0)
        {
            StringBuilder descriptionBuilder = new();

            descriptionBuilder.Append("Dernière modification le : ");
            descriptionBuilder.Append(_langDataCollection.GetDateTimeSinceLastChange().ToString("dd/MM/yyyy HH:mm"));
            descriptionBuilder.Append("+00:00\n");
            descriptionBuilder.Append(Formatter.MaskedUrl(Formatter.Bold(_langDataCollection.GetVersionFileName()), new Uri(_langDataCollection.GetVersionFileUrl())));
            descriptionBuilder.Append('\n');

            foreach (var langData in _langDataCollection)
            {
                descriptionBuilder.Append("- ");
                descriptionBuilder.Append(Formatter.MaskedUrl(langData.Name, new Uri(langData.GetFileUrl())));
                descriptionBuilder.Append(' ');
                descriptionBuilder.Append(Formatter.InlineCode(langData.Version.ToString()));
                descriptionBuilder.Append('\n');
            }

            embed.WithDescription(descriptionBuilder.ToString());
        }
        else
        {
            embed.WithDescription("void (°~°)");
        }

        return Task.FromResult(embed);
    }

    private DiscordSelectComponent Select1Builder()
    {
        List<DiscordSelectComponentOption> options = [];

        var types = Enum.GetValues<LangType>();
        for (var i = 0; i < types.Length && i < 25; i++)
        {
            options.Add(new(types[i].ToString(), GetPacket(types[i], _language), isDefault: (int)_type == i));
        }

        return new(InteractionManager.SelectComponentPacketBuilder(0), "Sélectionne un type pour l'afficher", options);
    }

    private DiscordSelectComponent Select2Builder()
    {
        List<DiscordSelectComponentOption> options = [];

        var languages = Enum.GetValues<LangLanguage>();
        for (var i = 0; i < languages.Length && i < 25; i++)
        {
            options.Add(new(languages[i].ToString(), GetPacket(_type, languages[i]), isDefault: (int)_language == i));
        }

        return new(InteractionManager.SelectComponentPacketBuilder(1), "Sélectionne une langue pour l'afficher", options);
    }
}
