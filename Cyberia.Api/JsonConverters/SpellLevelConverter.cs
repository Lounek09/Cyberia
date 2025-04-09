using Cyberia.Api.Data.Spells;
using Cyberia.Api.Enums;
using Cyberia.Api.Factories;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="SpellLevelData"/> objects.
/// </summary>
/// <remarks>
/// When reading JSON, this converter:
/// <list type="bullet">
///   <item>Expects a JSON array with 21 elements representing the properties of <see cref="SpellLevelData"/></item>
///   <item>Parses effects and critical effects using <see cref="EffectFactory.CreateMany"/> and effect areas using <see cref="EffectAreaFactory.CreateMany"/></item>
/// </list>
/// 
/// When writing JSON, it:
/// <list type="bullet">
///   <item>Serializes the <see cref="SpellLevelData"/> properties into a JSON array</item>
/// </list>
/// </remarks>
public sealed class SpellLevelConverter : JsonConverter<SpellLevelData>
{
    public override SpellLevelData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException($"Expected {JsonTokenType.StartArray} but got {reader.TokenType}");
        }

        var elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];
        if (elements.Length != 21)
        {
            throw new JsonException($"Expected 21 elements but got {elements.Length}");
        }

        var effects = JsonSerializer.Deserialize<JsonElement[]>(elements[0], options) ?? [];
        var criticalEffects = JsonSerializer.Deserialize<JsonElement[]>(elements[1], options) ?? [];
        var effectAreas = EffectAreaFactory.CreateMany(elements[15].GetString());

        var parsedEffects = EffectFactory.CreateMany(effects, effectAreas);
        effectAreas.RemoveRange(0, parsedEffects.Count);
        var parsedCriticalEffects = EffectFactory.CreateMany(criticalEffects, effectAreas);

        return new SpellLevelData
        {
            Effects = parsedEffects,
            CriticalEffects = parsedCriticalEffects,
            ActionPointCost = elements[2].GetInt32(),
            MinRange = elements[3].GetInt32(),
            MaxRange = elements[4].GetInt32(),
            CriticalHitRate = elements[5].GetInt32(),
            CriticalFailureRate = elements[6].GetInt32(),
            Linear = elements[7].GetBoolean(),
            LineOfSight = elements[8].GetBoolean(),
            NeedFreeCell = elements[9].GetBoolean(),
            AdjustableRange = elements[10].GetBoolean(),
            SpellLevelCategory = (SpellLevelCategory)elements[11].GetInt32(),
            CastPerTurn = elements[12].GetInt32(),
            CastPerPlayer = elements[13].GetInt32(),
            TurnsBetweenCast = elements[14].GetInt32(),
            RequiredStatesId = JsonSerializer.Deserialize<IReadOnlyList<int>>(elements[16], options) ?? [],
            ForbiddenStatesId = JsonSerializer.Deserialize<IReadOnlyList<int>>(elements[17], options) ?? [],
            NeededLevel = elements[18].GetInt32(),
            CricalFailureEndTheTurn = elements[19].GetBoolean(),
            Id = elements[20].GetInt32()
        };
    }

    public override void Write(Utf8JsonWriter writer, SpellLevelData value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        //TODO: Correctly serialize effects and critical effects of the spell level
        writer.WriteStartArray();
        writer.WriteEndArray();

        writer.WriteStartArray();
        writer.WriteEndArray();

        writer.WriteNumberValue(value.ActionPointCost);
        writer.WriteNumberValue(value.MinRange);
        writer.WriteNumberValue(value.MaxRange);
        writer.WriteNumberValue(value.CriticalHitRate);
        writer.WriteNumberValue(value.CriticalFailureRate);
        writer.WriteBooleanValue(value.Linear);
        writer.WriteBooleanValue(value.LineOfSight);
        writer.WriteBooleanValue(value.NeedFreeCell);
        writer.WriteBooleanValue(value.AdjustableRange);
        writer.WriteNumberValue((int)value.SpellLevelCategory);
        writer.WriteNumberValue(value.CastPerTurn);
        writer.WriteNumberValue(value.CastPerPlayer);
        writer.WriteNumberValue(value.TurnsBetweenCast);

        StringBuilder compressedEffectAreaBuilder = new((value.Effects.Count + value.CriticalEffects.Count) * 2);
        foreach (var effect in value.Effects)
        {
            compressedEffectAreaBuilder.Append(effect.EffectArea.ToCompressedString());
        }
        foreach (var effect in value.CriticalEffects)
        {
            compressedEffectAreaBuilder.Append(effect.EffectArea.ToCompressedString());
        }
        writer.WriteStringValue(compressedEffectAreaBuilder.ToString());

        writer.WriteStartArray();
        foreach (var stateId in value.RequiredStatesId)
        {
            writer.WriteNumberValue(stateId);
        }
        writer.WriteEndArray();

        writer.WriteStartArray();
        foreach (var stateId in value.ForbiddenStatesId)
        {
            writer.WriteNumberValue(stateId);
        }
        writer.WriteEndArray();

        writer.WriteNumberValue(value.NeededLevel);
        writer.WriteBooleanValue(value.CricalFailureEndTheTurn);
        writer.WriteNumberValue(value.Id);

        writer.WriteEndArray();
    }
}
