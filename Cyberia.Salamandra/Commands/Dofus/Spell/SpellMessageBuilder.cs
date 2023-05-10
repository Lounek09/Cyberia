using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Effects;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class SpellMessageBuilder : CustomMessageBuilder
    {
        private readonly Spell _spell;
        private readonly Breed? _breed;
        private readonly Incarnation? _incarnation;
        private int _level;
        private SpellLevel? _currentSpellLevel;

        public SpellMessageBuilder(Spell spell) :
            base()
        {
            _spell = spell;
            _breed = spell.GetBreed();
            _incarnation = spell.GetIncarnation();
            _level = spell.GetMaxLevelNumber();
            _currentSpellLevel = spell.GetSpellLevel(_level);
        }

        protected override async Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Spells, "Livre de sorts")
                .WithTitle($"{_spell.Name} ({_spell.Id}) - Rang {_level}")
                .WithDescription(string.IsNullOrEmpty(_spell.Description) ? "" : Formatter.Italic(_spell.Description))
                .WithThumbnail(await _spell.GetImgPath());

            if (_currentSpellLevel is not null)
            {
                embed.AddField(Constant.ZERO_WIDTH_SPACE, $"Niveau requis : {Formatter.Bold(_currentSpellLevel.NeededLevel.ToString())}", true);

                string range = $"{Formatter.Bold(_currentSpellLevel.MinRange.ToString())}{(_currentSpellLevel.MinRange == _currentSpellLevel.MaxRange ? "" : $" à {Formatter.Bold(_currentSpellLevel.MaxRange.ToString())}")} PO";
                string apCost = $"{Formatter.Bold(_currentSpellLevel.ActionPointCost.ToString())} PA";
                embed.AddField(Constant.ZERO_WIDTH_SPACE, $"{range}\n{apCost}", true);

                string category = _currentSpellLevel.GetCategoryName();
                if (!string.IsNullOrEmpty(category))
                    embed.AddField("Catégorie :", category, true);
                else
                    embed.AddField(Constant.ZERO_WIDTH_SPACE, Constant.ZERO_WIDTH_SPACE, true);

                List<State> requiredStates = _currentSpellLevel.GetRequiredStates();
                if (requiredStates.Count > 0)
                    embed.AddField("Etats requis :", string.Join(", ", requiredStates.Select(x => x.Name)), true);

                List<State> forbiddenStates = _currentSpellLevel.GetForbiddenStates();
                if (forbiddenStates.Count > 0)
                    embed.AddField("Etats interdits :", string.Join(", ", forbiddenStates.Select(x => x.Name)), true);

                if (_spell.Id == 101 || _spell.Id == 2083)
                    embed.AddField("Effets :", "Fuck roulette");
                else
                {
                    List<IEffect> effects = _currentSpellLevel.Effects;
                    List<IEffect> trapEffects = _currentSpellLevel.GetTrapEffects();
                    List<IEffect> glyphEffects = _currentSpellLevel.GetGlyphEffects();
                    List<IEffect> criticalEffects = _currentSpellLevel.CriticalEffects;

                    if (effects.Count == 0 && trapEffects.Count == 0 && glyphEffects.Count == 0 && criticalEffects.Count == 0)
                        embed.AddField(Constant.ZERO_WIDTH_SPACE, Constant.ZERO_WIDTH_SPACE);
                    else
                    {
                        if (effects.Count > 0)
                            embed.AddEffectFields("Effets :", effects);
                        if (trapEffects.Count > 0)
                            embed.AddEffectFields("Effets piege :", trapEffects);
                        if (glyphEffects.Count > 0)
                            embed.AddEffectFields("Effets glyphe :", glyphEffects);
                        if (criticalEffects.Count > 0)
                            embed.AddEffectFields("Effets critiques :", criticalEffects);
                    }
                }

                string caracteristics = $"""
                                         Probabilité de coup critique : {Formatter.Bold(_currentSpellLevel.CriticalHitRate == 0 ? "-" : $"1/{_currentSpellLevel.CriticalHitRate}")}
                                         Probabilité d'échec : {Formatter.Bold(_currentSpellLevel.CriticalFailureRate == 0 ? "-" : $"1/{_currentSpellLevel.CriticalFailureRate}")}
                                         Nb. de lancers par tour : {Formatter.Bold(_currentSpellLevel.LaunchCountByTurn == 0 ? "-" : _currentSpellLevel.LaunchCountByTurn.ToString())}
                                         Nb. de lancers par tour par joueur : {Formatter.Bold(_currentSpellLevel.LaunchCountByPlayerByTurn == 0 ? "-" : _currentSpellLevel.LaunchCountByPlayerByTurn.ToString())}
                                         Nb. de tours entre deux lancers : {Formatter.Bold(_currentSpellLevel.DelayBetweenLaunch == 0 ? "-" : _currentSpellLevel.DelayBetweenLaunch == 63 ? "inf." : _currentSpellLevel.DelayBetweenLaunch.ToString())}
                                         {(_spell.GlobalInterval ? "Intervalle de relance global" : "")}
                                         """;
                embed.AddField("Autres caractéristiques : ", caracteristics, true);

                caracteristics = $"""
                                   {Emojis.Bool(_currentSpellLevel.CanBoostRange)} Portée modifiable
                                   {Emojis.Bool(_currentSpellLevel.LineOfSight)} Ligne de vue
                                   {Emojis.Bool(_currentSpellLevel.LineOnly)} Lancer en ligne
                                   {Emojis.Bool(_currentSpellLevel.NeedFreeCell)} Cellules libres
                                   {Emojis.Bool(_currentSpellLevel.CricalFailureEndTheTurn)} EC fini le tour
                                   """;
                embed.AddField(Constant.ZERO_WIDTH_SPACE, caracteristics, true);
            }

            return embed;
        }

        private HashSet<DiscordButtonComponent> LevelButtons1Builder()
        {
            HashSet<DiscordButtonComponent> components = new();

            for (int i = 1; i < 6; i++)
            {
                if (_spell.GetSpellLevel(i) is not null)
                    components.Add(new(ButtonStyle.Primary, i.ToString(), i.ToString(), _level == i));
            }

            return components;
        }

        private HashSet<DiscordButtonComponent> LevelButtons2Builder()
        {
            HashSet<DiscordButtonComponent> components = new();

            if (_spell.GetSpellLevel(6) is not null)
                components.Add(new(ButtonStyle.Primary, "6", "6", _level == 6));

            if (_breed is not null)
                components.Add(new(ButtonStyle.Success, "breed", _breed.Name));

            if (_incarnation is not null)
                components.Add(new(ButtonStyle.Success, "incarnation", _incarnation.Name));

            return components;
        }

        protected override async Task<DiscordInteractionResponseBuilder> InteractionResponseBuilder()
        {
            DiscordInteractionResponseBuilder response = await base.InteractionResponseBuilder();

            HashSet<DiscordButtonComponent> levelButtons = LevelButtons1Builder();
            if (levelButtons.Count > 0)
                response.AddComponents(levelButtons);

            levelButtons = LevelButtons2Builder();
            if (levelButtons.Count > 0)
                response.AddComponents(levelButtons);

            return response;
        }

        protected override async Task<DiscordFollowupMessageBuilder> FollowupMessageBuilder()
        {
            DiscordFollowupMessageBuilder followupMessage = await base.FollowupMessageBuilder();

            HashSet<DiscordButtonComponent> levelButtons = LevelButtons1Builder();
            if (levelButtons.Count > 0)
                followupMessage.AddComponents(levelButtons);

            levelButtons = LevelButtons2Builder();
            if (levelButtons.Count > 0)
                followupMessage.AddComponents(levelButtons);

            return followupMessage;
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (int.TryParse(e.Id, out int level))
            {
                _level = level;
                _currentSpellLevel = _spell.GetSpellLevel(level);

                await UpdateInteractionResponse(e.Interaction);
                return true;
            }
            else if (e.Id.Equals("breed"))
            {
                if (_breed is not null)
                {
                    await new BreedMessageBuilder(_breed).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }
            else if (e.Id.Equals("incarnation"))
            {
                if (_incarnation is not null)
                {
                    await new IncarnationMessageBuilder(_incarnation).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
