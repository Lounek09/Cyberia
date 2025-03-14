﻿using Cyberia.Api.Data.Items;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class ItemWeaponDataConverter : JsonConverter<ItemWeaponData>
{
    public override ItemWeaponData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];

        if (elements.Length < 8)
        {
            throw new JsonException();
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
