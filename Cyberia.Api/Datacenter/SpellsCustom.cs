using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class SpellType
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        public SpellType()
        {
            Name = string.Empty;
        }
    }

    public sealed class SpellOrigin
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        public SpellOrigin()
        {
            Name = string.Empty;
        }
    }

    public sealed class SpellCategory
    {
        public const int TEMPORIS_BREED = 10;

        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        public SpellCategory()
        {
            Name = string.Empty;
        }
    }

    public sealed class SpellLevelCategory
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        public SpellLevelCategory()
        {
            Name = string.Empty;
        }
    }

    internal sealed class SpellsCustomData
    {
        [JsonPropertyName("CS.t")]
        public List<SpellType> SpellTypesCustom { get; init; }

        [JsonPropertyName("CS.o")]
        public List<SpellOrigin> SpellOriginsCustom { get; init; }

        [JsonPropertyName("CS.c")]
        public List<SpellCategory> SpellCategoriesCustom { get; init; }

        [JsonPropertyName("CSL.c")]
        public List<SpellLevelCategory> SpellLevelCategoriesCustom { get; init; }

        public SpellsCustomData()
        {
            SpellTypesCustom = new();
            SpellOriginsCustom = new();
            SpellCategoriesCustom = new();
            SpellLevelCategoriesCustom = new();
        }
    }
}
