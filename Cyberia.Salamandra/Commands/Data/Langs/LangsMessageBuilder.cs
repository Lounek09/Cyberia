using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Commands.Data
{
    public sealed class LangsMessageBuilder : ICustomMessageBuilder
    {
        public const string PACKET_HEADER = "LANG";
        public const int PACKET_VERSION = 1;

        private readonly LangType _type;
        private readonly Language _language;
        private readonly LangsData _langsData;

        public LangsMessageBuilder(LangType type, Language language)
        {
            _type = type;
            _language = language;
            _langsData = Bot.Instance.LangsWatcher.GetLangsByType(_type).GetLangsByLanguage(_language);
        }

        public static LangsMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 1 &&
                Enum.TryParse(parameters[0], out LangType langType) &&
                Enum.TryParse(parameters[1], out Language language))
            {
                return new LangsMessageBuilder(langType, language);
            }

            return null;
        }

        public static string GetPacket(LangType langType, Language language)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, langType, language);
        }

        public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
        {
            IDiscordMessageBuilder message = new T()
                .AddEmbed(await EmbedBuilder());

            DiscordSelectComponent select = Select1Builder();
            if (select.Options.Count > 0)
                message.AddComponents(select);

            DiscordSelectComponent select2 = Select2Builder();
            if (select2.Options.Count > 0)
                message.AddComponents(select2);

            return (T)message;
        }

        private Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Tools, "Langs")
                .WithTitle($"Langs {_type} en {_language}");

            
            if (_langsData.Langs.Count > 0)
            {
                StringBuilder content = new();

                content.AppendFormat("Dernière modification le : {0}+00:00\n", _langsData.GetDateTimeSinceLastModified().ToString("dd/MM/yyyy HH:mm"));
                content.AppendLine(Formatter.MaskedUrl(Formatter.Bold(_langsData.GetVersionFileName()), new Uri(_langsData.GetVersionFileUrl())));

                foreach (Lang lang in _langsData.Langs)
                    content.AppendLine($"- {Formatter.MaskedUrl(lang.Name, new Uri(lang.GetFileUrl()))} {Formatter.InlineCode(lang.Version.ToString())}");

                embed.WithDescription(content.ToString());
            }
            else
                embed.WithDescription("void (°~°)");

            return Task.FromResult(embed);
        }

        private DiscordSelectComponent Select1Builder()
        {
            HashSet<DiscordSelectComponentOption> options = new();

            LangType[] types = Enum.GetValues<LangType>();
            for (int i = 0; i < types.Length && i < 25; i++)
                options.Add(new(types[i].ToString(), GetPacket(types[i], _language), isDefault: (int)_type == i));

            return new(InteractionManager.SelectComponentPacketBuilder(0), "Sélectionne un type pour l'afficher", options);
        }

        private DiscordSelectComponent Select2Builder()
        {
            HashSet<DiscordSelectComponentOption> options = new();

            Language[] languages = Enum.GetValues<Language>();
            for (int i = 0; i < languages.Length && i < 25; i++)
                options.Add(new(languages[i].ToString(), GetPacket(_type, languages[i]), isDefault: (int)_language == i));

            return new(InteractionManager.SelectComponentPacketBuilder(1), "Sélectionne une langue pour l'afficher", options);
        }
    }
}
