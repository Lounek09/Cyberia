﻿using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Extensions.DSharpPlus;

/// <summary>
/// Provides extension methods for <see cref="AutoCompleteContext"/>.
/// </summary>
public static class AutocompleteContextExtensions
{
    /// <summary>
    /// Gets the value of the argument with the specified name.
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <param name="ctx">The context.</param>
    /// <param name="name">The name of the argument.</param>
    /// <returns>The value of the argument with the specified name; otherwise, the <see langword="default"/> value of <typeparamref name="T"/>.</returns>
    public static T? GetArgument<T>(this AutoCompleteContext ctx, string name)
    {
        var argument = ctx.Arguments.FirstOrDefault(x => x.Key.Name.Equals(name));
        if (argument.Value is T value)
        {
            return value;
        }

        return default;
    }
}
