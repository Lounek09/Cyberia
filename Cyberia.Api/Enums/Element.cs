namespace Cyberia.Api.Enums;

public enum Element
{
    Neutral,
    Strength,
    Intelligence,
    Chance,
    Agility
}

public static class ElementExtensions
{
    public static Element? GetFromCharacteristicId(int characteristicId)
    {
        return characteristicId switch
        {
            9 => Element.Neutral,
            10 => Element.Strength,
            13 => Element.Chance,
            14 => Element.Agility,
            15 => Element.Intelligence,
            _ => null
        };
    }
}
