using Cyberia.Api.DatacenterNS;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Parser.JsonConverter
{
    public sealed class QuestStepRewardsDataJsonConverter : JsonConverter<QuestStepRewardsData>
    {
        public override QuestStepRewardsData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is not JsonTokenType.StartArray)
                throw new JsonException("Invalid JSON format: expected an array.");

            JsonElement[]? elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options);
            if (elements is null || elements.Length != 6)
                throw new JsonException($"Invalid JSON format: expected an array of 6 values, but got a length of {elements?.Length}.");

            return new QuestStepRewardsData
            {
                Experience = elements[0].ValueKind is JsonValueKind.Null ? 0 : elements[0].GetInt32(),
                Kamas = elements[1].ValueKind is JsonValueKind.Null ? 0 : elements[1].GetInt32(),
                ItemsIdQuantities = (JsonSerializer.Deserialize<List<List<int>>>(elements[2].GetRawText(), options) ?? new())
                    .GroupBy(x => x[0])
                    .ToDictionary(x => x.Key, x => x.Sum(y => y[1])),
                EmotesId = JsonSerializer.Deserialize<List<int>>(elements[3].GetRawText(), options) ?? new(),
                JobsId = JsonSerializer.Deserialize<List<int>>(elements[4].GetRawText(), options) ?? new(),
                SpellsId = JsonSerializer.Deserialize<List<int>>(elements[5].GetRawText(), options) ?? new()
            };
        }

        public override void Write(Utf8JsonWriter writer, QuestStepRewardsData value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            if (value.Experience == 0)
                writer.WriteNullValue();
            else
                writer.WriteNumberValue(value.Experience);

            if (value.Kamas == 0)
                writer.WriteNullValue();
            else
                writer.WriteNumberValue(value.Kamas);

            if (value.ItemsIdQuantities.Count == 0)
                writer.WriteNullValue();
            else
            {
                writer.WriteStartArray();
                foreach (KeyValuePair<int, int> pair in value.ItemsIdQuantities)
                {
                    writer.WriteStartArray();
                    writer.WriteNumberValue(pair.Key);
                    writer.WriteNumberValue(pair.Value);
                    writer.WriteEndArray();
                }
                writer.WriteEndArray();
            }

            if (value.EmotesId.Count == 0)
                writer.WriteNullValue();
            else
            {
                writer.WriteStartArray();
                foreach (int emoteId in value.EmotesId)
                    writer.WriteNumberValue(emoteId);
                writer.WriteEndArray();
            }

            if (value.JobsId.Count == 0)
                writer.WriteNullValue();
            else
            {
                writer.WriteStartArray();
                foreach (int jobId in value.JobsId)
                    writer.WriteNumberValue(jobId);
                writer.WriteEndArray();
            }

            if (value.SpellsId.Count == 0)
                writer.WriteNullValue();
            else
            {
                writer.WriteStartArray();
                foreach (int spellId in value.SpellsId)
                    writer.WriteNumberValue(spellId);
                writer.WriteEndArray();
            }

            writer.WriteEndArray();
        }
    }
}
