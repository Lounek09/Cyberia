﻿using Cyberia.Api.Data.Items;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="ItemWeaponData"/> objects.
/// </summary>
/// <remarks>
/// - Expects a JSON array with 8 elements representing the properties of <see cref="ItemWeaponData"/>.<br />
/// - Deserializes the array into an <see cref="ItemWeaponData"/> instance.
/// </remarks>
public sealed class ItemWeaponDataConverter : JsonConverter<ItemWeaponData>
{
    public override ItemWeaponData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException($"Expected {JsonTokenType.StartArray} but got {reader.TokenType}.");
        }

        var elements = JsonElement.ParseValue(ref reader);

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
        throw new NotImplementedException();
    }
}
