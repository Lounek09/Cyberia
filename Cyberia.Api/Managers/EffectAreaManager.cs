namespace Cyberia.Api.Managers
{
    public readonly record struct EffectArea(int Id, int Size)
    {
        public string GetImagePath()
        {
            return $"{DofusApi.Config.CdnUrl}/images/effectareas/{Id}.png";
        }

        public string GetSize()
        {
            return Size >= 63 ? Resources.Infinity : Size.ToString();
        }

        public string GetDescription()
        {
            if (Id == EffectAreaManager.DefaultArea.Id)
            {
                return "";
            }

            string? effectAreaName = Resources.ResourceManager.GetString($"EffectArea.{Id}");
            if (effectAreaName is null)
            {
                Log.Warning("Unknown EffectArea {EffectAreaId}", Id);
                return $"{GetSize()} {PatternDecoder.Description(Resources.Unknown_Data, Id)}";
            }

            return $"{GetSize()} {effectAreaName}";
        }
    }

    public static class EffectAreaManager
    {
        public static readonly EffectArea DefaultArea = new(80, 0);

        public static EffectArea GetEffectArea(string value)
        {
            if (value.Length == 2)
            {
                return new(value[0], PatternDecoder.Base64(value[1]));
            }

            return new(-1, 0);
        }

        public static IEnumerable<EffectArea> GetEffectAreas(string values)
        {
            foreach (string value in values.SplitByLength(2))
            {
                yield return GetEffectArea(value);
            }
        }
    }
}
