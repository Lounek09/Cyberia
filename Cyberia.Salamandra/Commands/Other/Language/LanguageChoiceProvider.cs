using Cyberia.Api;
using Cyberia.Langzilla.Enums;

using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Trees;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Other.Language;

public sealed class LanguageChoiceProvider : IChoiceProvider
{
    private readonly DofusApiConfig _dofusApiConfig;

    public LanguageChoiceProvider(DofusApiConfig dofusApiConfig)
    {
        _dofusApiConfig = dofusApiConfig;
    }

    public ValueTask<IEnumerable<DiscordApplicationCommandOptionChoice>> ProvideAsync(CommandParameter parameter)
    {
        var choices = _dofusApiConfig.SupportedLanguages.Select(x =>
        {
            var language = x.ToStringFast();
            return new DiscordApplicationCommandOptionChoice(language, language);
        });

        return ValueTask.FromResult(choices);
    }
}
