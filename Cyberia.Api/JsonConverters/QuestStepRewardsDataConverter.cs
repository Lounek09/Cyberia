using Cyberia.Api.Data;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class QuestStepRewardsDataConverter : JsonConverter<QuestStepRewardsData>
{
    public override QuestStepRewardsData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];

        return new QuestStepRewardsData
        {
            Experience = elements[0].ValueKind is JsonValueKind.Null ? 0 : elements[0].GetInt32(),
            Kamas = elements[1].ValueKind is JsonValueKind.Null ? 0 : elements[1].GetInt32(),
            ItemsIdQuantities = (JsonSerializer.Deserialize<List<List<int>>>(elements[2].GetRawText(), options) ?? [])
                .GroupBy(x => x[0])
                .ToDictionary(x => x.Key, x => x.Sum(y => y[1]))
                .AsReadOnly(),
            EmotesId = (JsonSerializer.Deserialize<List<int>>(elements[3].GetRawText(), options) ?? []).AsReadOnly(),
            JobsId = (JsonSerializer.Deserialize<List<int>>(elements[4].GetRawText(), options) ?? []).AsReadOnly(),
            SpellsId = (JsonSerializer.Deserialize<List<int>>(elements[5].GetRawText(), options) ?? []).AsReadOnly()
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
