﻿using Cyberia.Api;
using Cyberia.Langzilla.Enums;

using DSharpPlus.Commands.Trees;

namespace Cyberia.Salamandra.Commands;

public sealed class SupportedCultureChoiceProvider : ChoiceProvider
{
    protected override IReadOnlyDictionary<string, object> InternalProvide(CommandParameter parameter)
    {
        return DofusApi.Config.SupportedLanguages.ToDictionary(x => x.ToStringFast(), x => (object)(int)x);
    }
}