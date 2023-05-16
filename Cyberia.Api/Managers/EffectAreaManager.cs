using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Managers
{
    public readonly struct Area
    {
        public int Id { get; init; }
        public int Size { get; init; }

        public Area(int id, int size)
        {
            Id = id;
            Size = size;
        }

        public EffectArea? GetEffectArea()
        {
            return DofusApi.Instance.Datacenter.EffectAreasData.GetEffectAreaBySymbol(Id);
        }

        public string GetSize()
        {
            return Size >= 63 ? "inf." : Size.ToString();
        }

        public string GetDescription()
        {
            if (Id == EffectAreaManager.BaseArea.Id)
                return "";

            EffectArea? effectArea = GetEffectArea();
            return $"{GetSize()} ({(effectArea is null ? $"Inconnu ({Id})" : effectArea.Name)})";
        }
    }

    public static class EffectAreaManager
    {
        public static readonly Area BaseArea = new(80, 0);

        public static Area GetArea(string value)
        {
            if (value.Length == 2)
                return new(value[0], PatternDecoder.Decode64(value[1]));

            return new(-1, 0);
        }

        public static IEnumerable<Area> GetAreas(string values)
        {
            foreach (string value in values.SplitByLength(2))
                yield return GetArea(value);
        }
    }
}
