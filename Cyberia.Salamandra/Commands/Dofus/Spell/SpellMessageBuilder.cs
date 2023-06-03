using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Effects;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class SpellMessageBuilder : ICustomMessageBuilder
    {
        public const string PACKET_HEADER = "S";
        public const int PACKET_VERSION = 1;

        private readonly Spell _spell;
        private readonly int _selectedLevel;
        private readonly SpellLevel? _spellLevel;
        private readonly SpellCategory? _spellCategory;
        private readonly Breed? _breed;
        private readonly Incarnation? _incarnation;

        public SpellMessageBuilder(Spell spell, int selectedLevel)
        {
            _spell = spell;
            _selectedLevel = selectedLevel;
            _spellLevel = spell.GetSpellLevel(selectedLevel);
            _spellCategory = spell.GetSpellCategory();
            _breed = spell.GetBreed();
            _incarnation = spell.GetIncarnation();
        }

        public static SpellMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 1 &&
                int.TryParse(parameters[0], out int spellId) &&
                int.TryParse(parameters[1], out int selectedLevel))
            {
                Spell? spell = Bot.Instance.Api.Datacenter.SpellsData.GetSpellById(spellId);
                if (spell is not null)
                    return new SpellMessageBuilder(spell, selectedLevel);
            }

            return null;
        }

        public static string GetPacket(int spellId, int selectedLevel)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, spellId, selectedLevel);
        }

        public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
        {
            IDiscordMessageBuilder message = new T()
                .AddEmbed(await EmbedBuilder());

            List<DiscordButtonComponent> buttons = Buttons1Builder();
            if (buttons.Count > 0)
                message.AddComponents(buttons);

            buttons = Buttons2Builder();
            if (buttons.Count > 0)
                message.AddComponents(buttons);

            return (T)message;
        }

        private async Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Spells, "Livre de sorts")
                .WithTitle($"{_spell.Name} ({_spell.Id}) - Rang {_selectedLevel}")
                .WithDescription(string.IsNullOrEmpty(_spell.Description) ? "" : Formatter.Italic(_spell.Description.Trim()))
                .WithThumbnail(await _spell.GetImagePath());

            if (_spellLevel is not null)
            {
                embed.AddField(Constant.ZERO_WIDTH_SPACE, $"Niveau requis : {Formatter.Bold(_spellLevel.NeededLevel.ToString())}", true);

                string range = $"{Formatter.Bold(_spellLevel.MinRange.ToString())}{(_spellLevel.MinRange == _spellLevel.MaxRange ? "" : $" à {Formatter.Bold(_spellLevel.MaxRange.ToString())}")} PO";
                string apCost = $"{Formatter.Bold(_spellLevel.ActionPointCost.ToString())} PA";
                embed.AddField(Constant.ZERO_WIDTH_SPACE, $"{range}\n{apCost}", true);

                if (_spellCategory is not null)
                    embed.AddField("Catégorie :", _spellCategory.Name, true);
                else
                    embed.AddField(Constant.ZERO_WIDTH_SPACE, Constant.ZERO_WIDTH_SPACE, true);

                List<State> requiredStates = _spellLevel.GetRequiredStates();
                if (requiredStates.Count > 0)
                    embed.AddField("Etats requis :", string.Join(", ", requiredStates.Select(x => x.Name)), true);

                List<State> forbiddenStates = _spellLevel.GetForbiddenStates();
                if (forbiddenStates.Count > 0)
                    embed.AddField("Etats interdits :", string.Join(", ", forbiddenStates.Select(x => x.Name)), true);

                if (_spell.Id == 101 || _spell.Id == 2083)
                    embed.AddField("Effets :", "Fuck roulette");
                else
                {
                    List<IEffect> effects = _spellLevel.Effects;
                    List<IEffect> trapEffects = _spellLevel.GetTrapEffects();
                    List<IEffect> glyphEffects = _spellLevel.GetGlyphEffects();
                    List<IEffect> criticalEffects = _spellLevel.CriticalEffects;

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
                                         Probabilité de coup critique : {Formatter.Bold(_spellLevel.CriticalHitRate == 0 ? "-" : $"1/{_spellLevel.CriticalHitRate}")}
                                         Probabilité d'échec : {Formatter.Bold(_spellLevel.CriticalFailureRate == 0 ? "-" : $"1/{_spellLevel.CriticalFailureRate}")}
                                         Nb. de lancers par tour : {Formatter.Bold(_spellLevel.LaunchCountByTurn == 0 ? "-" : _spellLevel.LaunchCountByTurn.ToString())}
                                         Nb. de lancers par tour par joueur : {Formatter.Bold(_spellLevel.LaunchCountByPlayerByTurn == 0 ? "-" : _spellLevel.LaunchCountByPlayerByTurn.ToString())}
                                         Nb. de tours entre deux lancers : {Formatter.Bold(_spellLevel.DelayBetweenLaunch == 0 ? "-" : _spellLevel.DelayBetweenLaunch == 63 ? "inf." : _spellLevel.DelayBetweenLaunch.ToString())}
                                         {(_spell.GlobalInterval ? "Intervalle de relance global" : "")}
                                         """;
                embed.AddField("Autres caractéristiques : ", caracteristics, true);

                caracteristics = $"""
                                  {Emojis.Bool(_spellLevel.CanBoostRange)} Portée modifiable
                                  {Emojis.Bool(_spellLevel.LineOfSight)} Ligne de vue
                                  {Emojis.Bool(_spellLevel.LineOnly)} Lancer en ligne
                                  {Emojis.Bool(_spellLevel.NeedFreeCell)} Cellules libres
                                  {Emojis.Bool(_spellLevel.CricalFailureEndTheTurn)} EC fini le tour
                                  """;
                embed.AddField(Constant.ZERO_WIDTH_SPACE, caracteristics, true);
            }

            return embed;
        }

        private List<DiscordButtonComponent> Buttons1Builder()
        {
            List<DiscordButtonComponent> components = new();

            for (int i = 1; i < 6; i++)
            {
                if (_spell.GetSpellLevel(i) is not null)
                    components.Add(new(ButtonStyle.Primary, GetPacket(_spell.Id, i), i.ToString(), _selectedLevel == i));
            }

            return components;
        }

        private List<DiscordButtonComponent> Buttons2Builder()
        {
            List<DiscordButtonComponent> components = new();

            if (_spell.GetSpellLevel(6) is not null)
                components.Add(new(ButtonStyle.Primary, GetPacket(_spell.Id, 6), "6", _selectedLevel == 6));

            if (_breed is not null)
                components.Add(BreedComponentsBuilder.BreedButtonBuilder(_breed));

            if (_incarnation is not null)
                components.Add(IncarnationComponentsBuilder.IncarnationButtonBuilder(_incarnation));

            return components;
        }
    }
}
