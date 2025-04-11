using Cyberia.Api.Data.Quests;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="QuestStepRewardsData"/> objects.
/// </summary>
/// <remarks>
/// When reading JSON, this converter:
/// <list type="bullet">
///   <item>Expects a JSON array with 6 elements representing the properties of <see cref="QuestStepRewardsData"/></item>
///   <item>Deserializes the array into a <see cref="QuestStepRewardsData"/> instance</item>
/// </list>
/// 
/// When writing JSON, it:
/// <list type="bullet">
///   <item>Serializes the <see cref="QuestStepRewardsData"/> properties into a JSON array</item>
/// </list>
/// </remarks>
public sealed class QuestStepRewardsDataConverter : JsonConverter<QuestStepRewardsData>
{
    public override QuestStepRewardsData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException($"Expected {JsonTokenType.StartArray} but got {reader.TokenType}.");
        }

        var elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];
        if (elements.Length != 6)
        {
            throw new JsonException($"Expected 6 elements but got {elements.Length}.");
        }

        Dictionary<int, int> itemsIdQuantities = [];
        if (elements[2].ValueKind == JsonValueKind.Array)
        {
            foreach (var item in elements[2].EnumerateArray())
            {
                var length = item.GetArrayLength();
                if (item.ValueKind != JsonValueKind.Array || length != 2)
                {
                    throw new JsonException($"Expected {JsonValueKind.Array} with 2 elements but got {item.ValueKind} with {length} elements.");
                }

                var itemId = item[0].GetInt32();
                var quantity = item[1].GetInt32();

                if (!itemsIdQuantities.TryAdd(itemId, quantity))
                {
                    itemsIdQuantities[itemId] += quantity;
                }
            }
        }

        return new QuestStepRewardsData
        {
            Experience = elements[0].GetInt32OrDefault(),
            Kamas = elements[1].GetInt32OrDefault(),
            ItemsIdQuantities = itemsIdQuantities.AsReadOnly(),
            EmotesId = JsonSerializer.Deserialize<IReadOnlyList<int>>(elements[3], options) ?? [],
            JobsId = JsonSerializer.Deserialize<IReadOnlyList<int>>(elements[4], options) ?? [],
            SpellsId = JsonSerializer.Deserialize<IReadOnlyList<int>>(elements[5], options) ?? []
        };
    }

    public override void Write(Utf8JsonWriter writer, QuestStepRewardsData value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        if (value.Experience == 0)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteNumberValue(value.Experience);
        }

        if (value.Kamas == 0)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteNumberValue(value.Kamas);
        }

        if (value.ItemsIdQuantities.Count == 0)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStartArray();

            foreach (var (itemId, quantity) in value.ItemsIdQuantities)
            {
                writer.WriteStartArray();

                writer.WriteNumberValue(itemId);
                writer.WriteNumberValue(quantity);

                writer.WriteEndArray();
            }

            writer.WriteEndArray();
        }

        if (value.EmotesId.Count == 0)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStartArray();

            foreach (var emoteId in value.EmotesId)
            {
                writer.WriteNumberValue(emoteId);
            }

            writer.WriteEndArray();
        }

        if (value.JobsId.Count == 0)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStartArray();

            foreach (var jobId in value.JobsId)
            {
                writer.WriteNumberValue(jobId);
            }

            writer.WriteEndArray();
        }

        if (value.SpellsId.Count == 0)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStartArray();

            foreach (var spellId in value.SpellsId)
            {
                writer.WriteNumberValue(spellId);
            }

            writer.WriteEndArray();
        }

        writer.WriteEndArray();
    }
}
