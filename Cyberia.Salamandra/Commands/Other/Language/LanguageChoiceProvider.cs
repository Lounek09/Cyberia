using Cyberia.Api;
using Cyberia.Langzilla.Enums;

using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Trees;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Other.Language;

public sealed class LanguageChoiceProvider : IChoiceProvider
{
    public ValueTask<IEnumerable<DiscordApplicationCommandOptionChoice>> ProvideAsync(CommandParameter parameter)
    {
        var choices = DofusApi.Config.SupportedLanguages.Select(x =>
        {
            var language = x.ToStringFast();
            return new DiscordApplicationCommandOptionChoice(language, language);
        });

        return ValueTask.FromResult(choices);
    }
}
