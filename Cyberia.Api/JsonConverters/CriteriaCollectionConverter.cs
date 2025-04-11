using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Criteria.Elements;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="CriteriaReadOnlyCollection"/> objects.
/// </summary>
/// <remarks>
/// - Expects a JSON string containing a compressed <see cref="CriteriaReadOnlyCollection"/> representation.<br />
/// - Parses this string into a structured <see cref="CriteriaReadOnlyCollection"/> using <see cref="CriterionFactory.CreateMany"/>.
/// </remarks>
public sealed class CriteriaReadOnlyCollectionConverter : JsonConverter<CriteriaReadOnlyCollection>
{
    public override CriteriaReadOnlyCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected {JsonTokenType.String} but got {reader.TokenType}.");
        }

        return CriterionFactory.CreateMany(reader.GetString() ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, CriteriaReadOnlyCollection values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
