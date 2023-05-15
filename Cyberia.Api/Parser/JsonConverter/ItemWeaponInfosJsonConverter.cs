using Cyberia.Api.DatacenterNS;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Parser.JsonConverter
{
    public sealed class ItemWeaponInfosJsonConverter : JsonConverter<ItemWeaponInfos>
    {
        public override ItemWeaponInfos Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is not JsonTokenType.StartArray)
                throw new JsonException("Invalid JSON format: expected an array.");

            JsonElement[]? elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options);
            if (elements is null || elements.Length != 8)
                throw new JsonException($"Invalid JSON format: expected an array of 8 values, but got a length of {elements?.Length}.");

            return new ItemWeaponInfos
            {
                CriticalBonus = elements[0].GetInt32(),
                ActionPointCost = elements[1].GetInt32(),
                MinRange = elements[2].GetInt32(),
                MaxRange = elements[3].GetInt32(),
                CriticalHitRate = elements[4].GetInt32(),
                CriticalFailureRate = elements[5].GetInt32(),
                LineOnly = elements[6].GetBoolean(),
                LineOfSight = elements[7].GetBoolean()
            };
        }

        public override void Write(Utf8JsonWriter writer, ItemWeaponInfos value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            writer.WriteNumberValue(value.CriticalBonus);
            writer.WriteNumberValue(value.ActionPointCost);
            writer.WriteNumberValue(value.MinRange);
            writer.WriteNumberValue(value.MaxRange);
            writer.WriteNumberValue(value.CriticalHitRate);
            writer.WriteNumberValue(value.CriticalFailureRate);
            writer.WriteBooleanValue(value.LineOnly);
            writer.WriteBooleanValue(value.LineOfSight);

            writer.WriteEndArray();
        }
    }
}
