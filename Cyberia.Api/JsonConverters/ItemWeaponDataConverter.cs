using Cyberia.Api.Data.Items;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="ItemWeaponData"/> objects.
/// </summary>
/// <remarks>
/// When reading JSON, this converter:
/// <list type="bullet">
///   <item>Expects a JSON array with 8 elements representing the properties of <see cref="ItemWeaponData"/></item>
///   <item>Deserializes the array into an <see cref="ItemWeaponData"/> instance</item>
/// </list>
/// 
/// When writing JSON, it:
/// <list type="bullet">
///   <item>Serializes the <see cref="ItemWeaponData"/> properties into a JSON array</item>
/// </list>
/// </remarks>
public sealed class ItemWeaponDataConverter : JsonConverter<ItemWeaponData>
{
    public override ItemWeaponData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException($"Expected {JsonTokenType.StartArray} but got {reader.TokenType}");
        }

        var elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];

        if (elements.Length != 8)
        {
            throw new JsonException($"Expected 8 elements but got {elements.Length}");
        }

        return new ItemWeaponData
        {
            CriticalBonus = elements[0].GetInt32(),
            ActionPointCost = elements[1].GetInt32(),
            MinRange = elements[2].GetInt32(),
            MaxRange = elements[3].GetInt32(),
            CriticalHitRate = elements[4].GetInt32(),
            CriticalFailureRate = elements[5].GetInt32(),
            Linear = elements[6].GetBoolean(),
            LineOfSight = elements[7].GetBoolean()
        };
    }

    public override void Write(Utf8JsonWriter writer, ItemWeaponData value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        writer.WriteNumberValue(value.CriticalBonus);
        writer.WriteNumberValue(value.ActionPointCost);
        writer.WriteNumberValue(value.MinRange);
        writer.WriteNumberValue(value.MaxRange);
        writer.WriteNumberValue(value.CriticalHitRate);
        writer.WriteNumberValue(value.CriticalFailureRate);
        writer.WriteBooleanValue(value.Linear);
        writer.WriteBooleanValue(value.LineOfSight);

        writer.WriteEndArray();
    }
}
