using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Managers
{
    public readonly struct Area
    {
        public char Symbol { get; init; }
        public int Size { get; init; }

        public Area(char symbol, int size)
        {
            Symbol = symbol;
            Size = size;
        }

        public EffectArea? GetEffectArea()
        {
            return DofusApi.Instance.Datacenter.EffectAreasData.GetEffectAreaBySymbol(Symbol);
        }

        public string GetSize()
        {
            return Size >= 63 ? "inf." : Size.ToString();
        }

        public string GetDescription()
        {
            if (Symbol == EffectAreaManager.BaseArea.Symbol)
                return "";

            EffectArea? effectArea = GetEffectArea();
            return $"{GetSize()} ({(effectArea is null ? Symbol : effectArea.Name)})";
        }
    }

    public static class EffectAreaManager
    {
        public static readonly Area BaseArea = new('P', 0);

        public static Area GetArea(string value)
        {
            if (value.Length == 2)
                return new(value[0], PatternDecoder.Decode64(value[1]));

            return new('?', 0);
        }

        public static IEnumerable<Area> GetAreas(string values)
        {
            foreach (string value in values.SplitByLength(2))
                yield return GetArea(value);
        }
    }
}
