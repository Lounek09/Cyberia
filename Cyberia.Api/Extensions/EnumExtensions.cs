namespace Cyberia.Api.Extensions;

public static class EnumExtensions
{
    public static string GetDescription<T>(this T value)
        where T : Enum
    {
        var description = ApiTranslations.ResourceManager.GetString($"{typeof(T).Name}.{value}");
        if (description is not null)
        {
            return description;
        }

        return Translation.Format(ApiTranslations.Unknown_Data, value);
    }
}
