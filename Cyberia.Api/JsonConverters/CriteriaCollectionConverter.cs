using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Criteria.Elements;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="CriteriaReadOnlyCollection"/> objects.
/// </summary>
/// <remarks>
/// When reading JSON, this converter:
/// <list type="bullet">
///   <item>Expects a JSON string containing a compressed <see cref="CriteriaReadOnlyCollection"/> representation</item>
///   <item>Parses this string into a structured <see cref="CriteriaReadOnlyCollection"/> using <see cref="CriterionFactory.CreateMany"/></item>
/// </list>
/// 
/// When writing JSON, it:
/// <list type="bullet">
///   <item>Converts the <see cref="CriteriaReadOnlyCollection"/> back to its compressed string representation</item>
/// </list>
/// </remarks>
public sealed class CriteriaReadOnlyCollectionConverter : JsonConverter<CriteriaReadOnlyCollection>
{
    public override CriteriaReadOnlyCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected {JsonTokenType.String} but got {reader.TokenType}");
        }

        return CriterionFactory.CreateMany(reader.GetString() ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, CriteriaReadOnlyCollection values, JsonSerializerOptions options)
    {
        //TODO: Implement the write method to serialize CriteriaReadOnlyCollection to JSON.
        throw new NotImplementedException();
    }
}
