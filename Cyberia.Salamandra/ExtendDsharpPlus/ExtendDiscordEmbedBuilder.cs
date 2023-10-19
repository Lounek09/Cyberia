using Cyberia.Api;
using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Factories.QuestObjectives;
using Cyberia.Api.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.DsharpPlus
{
    public static class ExtendDiscordEmbedBuilder
    {
        public static DiscordEmbedBuilder AddFields(this DiscordEmbedBuilder embed, string name, IEnumerable<string> rows, bool inline = false)
        {
            string content = "";

            foreach (string row in rows)
            {
                if (row.Length > 1024)
                    throw new ArgumentException("One row exceed 1024 characters and embed field value cannot exceed this length.");

                if (content.Length + row.Length > 1024)
                {
                    embed.AddField(name, content[..^1], inline);
                    content = "";
                }

                content += $"{row}\n";
            }

            return embed.AddField(name, content[..^1], inline);
        }

        public static DiscordEmbedBuilder AddEffectFields(this DiscordEmbedBuilder embed, string name, IEnumerable<IEffect> effects, bool inline = false)
        {
            List<string> effectsParse = new();

            foreach (IEffect effect in effects)
            {
                string emoji = effect switch
                {
                    AddStateEffect addStateEffect => Emojis.State(addStateEffect.StateId),
                    RemoveStateEffect removeStateEffect => Emojis.State(removeStateEffect.StateId),
                    _ => Emojis.Effect(effect.EffectId)
                };

                string effectDescription = effect.GetDescription().ToString(x => Formatter.Bold(Formatter.Sanitize(x)));

                string areaInfo = (effect.EffectArea.Size == EffectAreaManager.DefaultArea.Size) ? "" : $" - {Emojis.Area(effect.EffectArea.Id)} {effect.EffectArea.GetSize()}";

                string effectParse = $"{emoji} {effectDescription}{areaInfo}";

                if (effect.Criteria.Count > 0)
                {
                    List<string> criteriaParse = GetCriteriaParse(effect.Criteria);
                    effectParse += $" {Formatter.InlineCode(string.Join(' ', criteriaParse))}";
                }

                effectsParse.Add(effectParse);
            }

            return embed.AddFields(name, effectsParse, inline);
        }

        public static DiscordEmbedBuilder AddCriteriaFields(this DiscordEmbedBuilder embed, IEnumerable<ICriteriaElement> criteria, bool inline = false)
        {
            return embed.AddFields("Conditions : ", GetCriteriaParse(criteria, x => Formatter.Bold(Formatter.Sanitize(x))), inline);
        }

        public static DiscordEmbedBuilder AddQuestObjectiveFields(this DiscordEmbedBuilder embed, IEnumerable<IQuestObjective> questObjectives, bool inline = false)
        {
            List<string> questObjectivesParse = new();

            foreach (IQuestObjective questObjective in questObjectives)
                questObjectivesParse.Add(questObjective.GetDescription().ToString(x => Formatter.Bold(Formatter.Sanitize(x))));

            return embed.AddFields("Objectifs :", questObjectivesParse, inline);
        }

        public static DiscordEmbedBuilder WithCraftDescription(this DiscordEmbedBuilder embed, CraftData craftData, int qte, bool recursive)
        {
            List<string> result = new();

            Dictionary<int, int> ingredients = recursive ? craftData.GetRecursiveIngredients(qte) : craftData.GetIngredients(qte);
            foreach (KeyValuePair<int, int> ingredient in ingredients)
            {
                string itemName = Formatter.Sanitize(Bot.Instance.Api.Datacenter.ItemsData.GetItemNameById(ingredient.Key));

                if (!recursive)
                {
                    CraftData? subCraftData = Bot.Instance.Api.Datacenter.CraftsData.GetCraftDataById(ingredient.Key);
                    if (subCraftData is not null)
                        itemName = Formatter.Bold(itemName);
                }

                result.Add($"{Formatter.Bold(ingredient.Value.ToStringThousandSeparator())}x {itemName}");
            }

            return embed.WithDescription(string.Join('\n', result));
        }

        public static DiscordEmbedBuilder AddCraftField(this DiscordEmbedBuilder embed, CraftData craftData, int qte, bool inline = false)
        {
            List<string> result = new();

            foreach (KeyValuePair<int, int> ingredient in craftData.GetIngredients(qte))
            {
                string itemName = Formatter.Sanitize(Bot.Instance.Api.Datacenter.ItemsData.GetItemNameById(ingredient.Key));

                result.Add($"{Formatter.Bold(ingredient.Value.ToStringThousandSeparator())}x {itemName}");
            }

            return embed.AddField("Craft :", string.Join(" + ", result), inline);
        }

        public static DiscordEmbedBuilder AddWeaponInfosField(this DiscordEmbedBuilder embed, ItemWeaponInfosData itemWeaponInfosData, bool twoHanded, ItemTypeData? itemTypeData, bool inline = false)
        {
            StringBuilder builder = new();

            builder.AppendFormat("PA : {0}\n", itemWeaponInfosData.ActionPointCost);
            builder.AppendFormat("Portée : {0} à {1}\n", itemWeaponInfosData.MinRange, itemWeaponInfosData.MaxRange);

            if (itemWeaponInfosData.CriticalBonus != 0)
                builder.AppendFormat("Bonus coups critique : {0}\n", itemWeaponInfosData.CriticalBonus);

            if (itemWeaponInfosData.CriticalHitRate != 0)
            {
                builder.AppendFormat("Critique : 1/{0}", itemWeaponInfosData.CriticalHitRate);
                builder.Append(itemWeaponInfosData.CriticalFailureRate != 0 ? " - " : "\n");
            }

            if (itemWeaponInfosData.CriticalFailureRate != 0)
                builder.AppendFormat("Échec : 1/{0}\n", itemWeaponInfosData.CriticalFailureRate);

            if (itemWeaponInfosData.LineOnly)
                builder.AppendLine("Lancer en ligne uniquement");

            if (!itemWeaponInfosData.LineOfSight && itemWeaponInfosData.MaxRange > 1)
                builder.AppendLine("Ne possède pas de ligne de vue");

            builder.Append(twoHanded ? "Arme à deux mains" : "Arme à une main");

            if (itemTypeData is not null && itemTypeData.EffectArea.Id != EffectAreaManager.DefaultArea.Id)
                builder.AppendFormat("\nZone : {0} {1}", Emojis.Area(itemTypeData.EffectArea.Id), itemTypeData.EffectArea.GetDescription());

            return embed.AddField("Caractéristiques :", builder.ToString(), inline);
        }

        private static List<string> GetCriteriaParse(IEnumerable<ICriteriaElement> criteriaElements, Func<string, string>? parametersDecorator = null)
        {
            List<string> criteriaParse = new() { "" };

            foreach (ICriteriaElement element in criteriaElements)
            {
                switch (element)
                {
                    case LogicalOperatorCriteriaElement logicalOperator:
                        criteriaParse[^1] += $"{logicalOperator.GetDescription()} ";
                        break;
                    case CollectionCriteriaElement collection:
                        List<string> subCriteriaParse = GetCriteriaParse(collection.Criteria, parametersDecorator);
                        criteriaParse[^1] += "(" + subCriteriaParse[0];

                        if (subCriteriaParse.Count > 1)
                            criteriaParse.AddRange(subCriteriaParse.GetRange(1, subCriteriaParse.Count - 1));

                        criteriaParse[^1] += ")";
                        criteriaParse.Add("");
                        break;
                    case ICriterion criterion:
                        criteriaParse[^1] += parametersDecorator is null ? criterion.GetDescription() : criterion.GetDescription().ToString(parametersDecorator);
                        criteriaParse.Add("");
                        break;
                }
            }

            if (string.IsNullOrEmpty(criteriaParse[^1]))
                criteriaParse.RemoveAt(criteriaParse.Count - 1);

            return criteriaParse;
        }
    }
}
