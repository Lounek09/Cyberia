using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Admin.Emoji;

public sealed class EmojiAutocompleteProvider : IAutoCompleteProvider
{
    public ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        var choices = EmojisService.GetEmojisByName(ctx.UserInput ?? string.Empty)
            .Take(Constant.MaxChoice)
            .Select(x =>
            {
                return new DiscordAutoCompleteChoice($"{x.Name} ({x.Id})", x.Name);
            });

        return ValueTask.FromResult(choices);
    }
}
