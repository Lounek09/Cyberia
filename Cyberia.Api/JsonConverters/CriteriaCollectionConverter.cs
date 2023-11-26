using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Criteria;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters
{
    public sealed class CriteriaCollectionConverter : JsonConverter<CriteriaCollection>
    {
        public override CriteriaCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return CriterionFactory.GetCriteria(reader.GetString() ?? string.Empty);
        }

        public override void Write(Utf8JsonWriter writer, CriteriaCollection values, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
