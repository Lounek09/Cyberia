using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Data
{
    public class LangsMessageBuilder : CustomMessageBuilder
    {
        private LangType _type;
        private Language _language;

        public LangsMessageBuilder(LangType type, Language language, DiscordUser user) :
            base(user)
        {
            _type = type;
            _language = language;
        }

        protected override Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Tools, "Langs")
                                        .WithTitle($"Langs {_type} en {_language}");

            LangsData data = Bot.Instance.DofusLangs.GetLangsData(_type, _language);
            if (data.Langs.Count > 0)
            {
                HashSet<string> content = new()
                {
                    $"Dernière modification le : {data.GetLastModifiedDateTime(): dd/MM/yyyy HH:mm}+00:00",
                    Formatter.MaskedUrl(Formatter.Bold(data.GetVersionFileName()), new Uri(data.GetVersionFileUrl())),
                    ""
                };

                foreach (Lang lang in data.Langs)
                    content.Add($"- {Formatter.MaskedUrl(Formatter.Bold(lang.Name), new Uri(lang.GetFileUrl()))} {Formatter.InlineCode(lang.Version.ToString())}");

                embed.WithDescription(string.Join("\n", content));
            }
            else
                embed.WithDescription("");

            return Task.FromResult(embed);
        }

        private DiscordSelectComponent SelectTypeBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();

            LangType[] types = Enum.GetValues<LangType>();
            for (int i = 0; i < types.Length && i < 25; i++)
                options.Add(new(types[i].ToString(), $"{types[i]}|{_language}", isDefault: (int)_type == i));

            return new("selectType", "Sélectionne un type pour l'afficher", options);
        }

        private DiscordSelectComponent SelectLanguageBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();

            Language[] languages = Enum.GetValues<Language>();
            for (int i = 0; i < languages.Length && i < 25; i++)
                options.Add(new(languages[i].ToString(), $"{_type}|{languages[i]}", isDefault: (int)_language == i));

            return new("selectLanguage", "Sélectionne une langue pour l'afficher", options);
        }

        protected override async Task<DiscordInteractionResponseBuilder> InteractionResponseBuilder()
        {
            DiscordInteractionResponseBuilder response = await base.InteractionResponseBuilder();

            DiscordSelectComponent select = SelectTypeBuilder();
            if (select.Options.Count > 1)
                response.AddComponents(select);

            DiscordSelectComponent select2 = SelectLanguageBuilder();
            if (select2.Options.Count > 1)
                response.AddComponents(select2);

            return response;
        }

        protected override async Task<DiscordFollowupMessageBuilder> FollowupMessageBuilder()
        {
            DiscordFollowupMessageBuilder followupMessage = await base.FollowupMessageBuilder();

            DiscordSelectComponent select = SelectTypeBuilder();
            if (select.Options.Count > 1)
                followupMessage.AddComponents(select);

            DiscordSelectComponent select2 = SelectLanguageBuilder();
            if (select2.Options.Count > 1)
                followupMessage.AddComponents(select2);

            return followupMessage;
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            switch (e.Id)
            {
                case "selectType":
                case "selectLanguage":
                    string[] args = e.Interaction.Data.Values.First().Split("|");

                    _type = Enum.Parse<LangType>(args[0]);
                    _language = Enum.Parse<Language>(args[1]);

                    await UpdateInteractionResponse(e.Interaction);
                    return true;
                default:
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                    return false;
            }
        }
    }
}
