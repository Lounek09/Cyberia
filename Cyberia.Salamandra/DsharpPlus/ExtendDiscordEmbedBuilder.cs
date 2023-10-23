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
            StringBuilder builder = new();

            foreach (string row in rows)
            {
                if (row.Length > 1024)
                    throw new ArgumentException("One row exceeds 1024 characters and embed field value cannot exceed this length.");

                if (builder.Length + row.Length > 1024)
                {
                    builder.Length--;
                    embed.AddField(name, builder.ToString(), inline);
                    builder.Clear();
                }

                builder.Append(row);
                builder.Append('\n'); //Not using builder.AppendLine() because it adds \r\n in windows
            }

            builder.Length--;
            return embed.AddField(name, builder.ToString(), inline);
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

                string areaInfo = effect.EffectArea.Size == EffectAreaManager.DefaultArea.Size ? "" : $" - {Emojis.Area(effect.EffectArea.Id)} {effect.EffectArea.GetSize()}";

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

        public static DiscordEmbedBuilder AddCriteriaFields(this DiscordEmbedBuilder embed, CriteriaCollection criteria, bool inline = false)
        {
            List<string> criteriaParse = GetCriteriaParse(criteria, x => Formatter.Bold(Formatter.Sanitize(x)));
            return embed.AddFields("Conditions : ", criteriaParse, inline);
        }

        public static DiscordEmbedBuilder AddQuestObjectiveFields(this DiscordEmbedBuilder embed, IEnumerable<IQuestObjective> questObjectives, bool inline = false)
        {
            List<string> questObjectivesParse = new();

            foreach (IQuestObjective questObjective in questObjectives)
            {
                string questObjectiveDescription = questObjective.GetDescription().ToString(x => Formatter.Bold(Formatter.Sanitize(x)));
                questObjectivesParse.Add(questObjectiveDescription);
            }

            return embed.AddFields("Objectifs :", questObjectivesParse, inline);
        }

        public static DiscordEmbedBuilder WithCraftDescription(this DiscordEmbedBuilder embed, CraftData craftData, int qte, bool recursive)
        {
            List<string> result = new();

            Dictionary<int, int> ingredients = recursive ? craftData.GetRecursiveIngredients(qte) : craftData.GetIngredients(qte);
            foreach (KeyValuePair<int, int> ingredient in ingredients)
            {
                string quantity = Formatter.Bold(ingredient.Value.ToStringThousandSeparator());
                string itemName = Formatter.Sanitize(Bot.Instance.Api.Datacenter.ItemsData.GetItemNameById(ingredient.Key));

                if (!recursive)
                {
                    CraftData? subCraftData = Bot.Instance.Api.Datacenter.CraftsData.GetCraftDataById(ingredient.Key);
                    if (subCraftData is not null)
                        itemName = Formatter.Bold(itemName);
                }

                result.Add($"{quantity}x {itemName}");
            }

            return embed.WithDescription(string.Join('\n', result));
        }

        public static DiscordEmbedBuilder AddCraftField(this DiscordEmbedBuilder embed, CraftData craftData, int qte, bool inline = false)
        {
            List<string> result = new();

            foreach (KeyValuePair<int, int> ingredient in craftData.GetIngredients(qte))
            {
                string quantity = Formatter.Bold(ingredient.Value.ToStringThousandSeparator());
                string itemName = Formatter.Sanitize(Bot.Instance.Api.Datacenter.ItemsData.GetItemNameById(ingredient.Key));

                result.Add($"{quantity}x {itemName}");
            }

            return embed.AddField("Craft :", string.Join(" + ", result), inline);
        }

        public static DiscordEmbedBuilder AddWeaponInfosField(this DiscordEmbedBuilder embed, ItemWeaponData itemWeaponData, bool twoHanded, ItemTypeData? itemTypeData, bool inline = false)
        {
            StringBuilder builder = new();

            string actionPointCost = Formatter.Bold(itemWeaponData.ActionPointCost.ToString());
            builder.AppendFormat("PA : {0}\n", actionPointCost);

            string minRange = Formatter.Bold(itemWeaponData.MinRange.ToString());
            string maxRange = Formatter.Bold(itemWeaponData.MaxRange.ToString());
            builder.AppendFormat("Portée : {0} à {1}\n", minRange, maxRange);

            if (itemWeaponData.CriticalBonus != 0)
            {
                string criticalBonus = Formatter.Bold(itemWeaponData.CriticalBonus.ToString());
                builder.AppendFormat("Bonus coups critique : {0}\n", criticalBonus);
            }

            if (itemWeaponData.CriticalHitRate != 0)
            {
                string criticalHitRate = Formatter.Bold(itemWeaponData.CriticalHitRate.ToString());
                builder.AppendFormat("Critique : 1/{0}", criticalHitRate);
                builder.Append(itemWeaponData.CriticalFailureRate != 0 ? " - " : "\n");
            }

            if (itemWeaponData.CriticalFailureRate != 0)
            {
                string criticalFailureRate = Formatter.Bold(itemWeaponData.CriticalFailureRate.ToString());
                builder.AppendFormat("Échec : 1/{0}\n", criticalFailureRate);
            }


            if (itemWeaponData.LineOnly)
                builder.AppendLine("Lancer en ligne uniquement");

            if (!itemWeaponData.LineOfSight && itemWeaponData.MaxRange > 1)
                builder.AppendLine("Ne possède pas de ligne de vue");

            builder.Append(twoHanded ? "Arme à deux mains" : "Arme à une main");

            if (itemTypeData is not null && itemTypeData.EffectArea.Id != EffectAreaManager.DefaultArea.Id)
            {
                string emoji = Emojis.Area(itemTypeData.EffectArea.Id);
                string description = itemTypeData.EffectArea.GetDescription();
                builder.AppendFormat("\nZone : {0} {1}", emoji, description);
            }

            return embed.AddField("Caractéristiques :", builder.ToString(), inline);
        }

        public static DiscordEmbedBuilder AddPetField(this DiscordEmbedBuilder embed, PetData petData, bool inline = false)
        {
            StringBuilder builder = new();

            if (petData.MinFoodInterval.HasValue && petData.MaxFoodInterval.HasValue)
            {
                string minHoursInterval = Formatter.Bold(petData.MinFoodInterval.Value.TotalHours.ToString());
                string maxHoursInterval = Formatter.Bold(petData.MaxFoodInterval.Value.TotalHours.ToString());
                builder.AppendFormat("Repas entre {0}h et {1}h\n", minHoursInterval, maxHoursInterval);
            }

            foreach (PetFoodsData petFoodsData in petData.Foods)
            {
                if (petFoodsData.Effect is not null)
                {
                    builder.AppendFormat("{0} {1} :\n", Emojis.Effect(petFoodsData.Effect.EffectId), Formatter.Bold(petFoodsData.Effect.GetDescription()));

                    if (petFoodsData.ItemsId.Count > 0)
                    {
                        IEnumerable<string> itemsName = petFoodsData.ItemsId.Select(x => Bot.Instance.Api.Datacenter.ItemsData.GetItemNameById(x));
                        builder.AppendLine(string.Join(", ", itemsName));
                    }

                    if (petFoodsData.ItemTypesId.Count > 0)
                    {
                        IEnumerable<string> itemTypesName = petFoodsData.ItemTypesId.Select(x => Bot.Instance.Api.Datacenter.ItemsData.GetItemTypeNameById(x));
                        builder.AppendLine(string.Join(", ", itemTypesName));
                    }


                    if (petFoodsData.MonstersIdQuantities.Count > 0)
                    {
                        foreach (IGrouping<int, KeyValuePair<int, int>> group in petFoodsData.MonstersIdQuantities.GroupBy(x => x.Value))
                        {
                            string quantity = Formatter.Bold(group.Key.ToString());
                            IEnumerable<string> monsterName = group.Select(x => Bot.Instance.Api.Datacenter.MonstersData.GetMonsterNameById(x.Key));
                            builder.AppendFormat("{0}x {1}\n", quantity, string.Join(", ", monsterName));
                        }
                    }
                }
            }

            return embed.AddField("Familier :", builder.ToString(), inline);
        }

        private static List<string> GetCriteriaParse(CriteriaCollection criteria, Func<string, string>? parametersDecorator = null)
        {
            List<string> criteriaParse = new() { "" };

            foreach (ICriteriaElement element in criteria)
            {
                switch (element)
                {
                    case CriteriaLogicalOperator logicalOperator:
                        criteriaParse[^1] += $"{logicalOperator.GetDescription()} ";
                        break;
                    case CriteriaCollection subCriteria:
                        List<string> subCriteriaParse = GetCriteriaParse(subCriteria, parametersDecorator);
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
