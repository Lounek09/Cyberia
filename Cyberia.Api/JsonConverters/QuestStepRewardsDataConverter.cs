using Cyberia.Api.Data.Quests;
using Cyberia.Utils.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class QuestStepRewardsDataConverter : JsonConverter<QuestStepRewardsData>
{
    public override QuestStepRewardsData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];

        if (elements.Length < 6)
        {
            throw new JsonException();
        }

        var itemsIdQuantities = JsonSerializer.Deserialize<int[][]>(elements[2], options) ?? [];
        Dictionary<int, int> itemsIdQuantitiesDictionary = [];

        foreach (var item in itemsIdQuantities)
        {
            if (item.Length != 2)
            {
                throw new JsonException();
            }

            if (itemsIdQuantitiesDictionary.ContainsKey(item[0]))
            {
                itemsIdQuantitiesDictionary[item[0]] += item[1];
            }
            else
            {
                itemsIdQuantitiesDictionary[item[0]] = item[1];
            }
        }

        return new QuestStepRewardsData
        {
            Experience = elements[0].GetInt32OrDefault(),
            Kamas = elements[1].GetInt32OrDefault(),
            ItemsIdQuantities = itemsIdQuantitiesDictionary,
            EmotesId = JsonSerializer.Deserialize<List<int>>(elements[3], options) ?? [],
            JobsId = JsonSerializer.Deserialize<List<int>>(elements[4], options) ?? [],
            SpellsId = JsonSerializer.Deserialize<List<int>>(elements[5], options) ?? []
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

            foreach (var pair in value.ItemsIdQuantities)
            {
                writer.WriteStartArray();

                writer.WriteNumberValue(pair.Key);
                writer.WriteNumberValue(pair.Value);

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
