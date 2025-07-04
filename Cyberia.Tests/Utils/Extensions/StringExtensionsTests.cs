﻿namespace Cyberia.Tests.Utils.Extensions;

[TestClass]
public class StringExtensionsTests
{
    #region Capitalize

    [TestMethod]
    public void Capitalize_WithEmptyString_ReturnsEmpty()
    {
        var value = string.Empty;

        var result = value.Capitalize();

        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void Capitalize_WithSingleCharacter_ReturnsCapitalized()
    {
        var value = "k";

        var result = value.Capitalize();

        Assert.AreEqual("K", result);
    }

    [TestMethod]
    public void Capitalize_WithMultipleCharacters_ReturnsFirstCapitalized()
    {
        var value = "bouftou";

        var result = value.Capitalize();

        Assert.AreEqual("Bouftou", result);
    }

    #endregion

    #region WithMaxLength

    [TestMethod]
    public void WithMaxLength_WithExactLength_ReturnsOriginalString()
    {
        var value = "Bouftou";

        var result = value.WithMaxLength(7);

        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void WithMaxLength_WithShorterLength_ReturnsTruncatedString()
    {
        var value = "Bouftou Royal";

        var result = value.WithMaxLength(7);

        Assert.AreEqual("Bouftou", result);
    }

    [TestMethod]
    public void WithMaxLength_WithLongerLength_ReturnsOriginalString()
    {
        var value = "Bouftou";

        var result = value.WithMaxLength(10);

        Assert.AreEqual(value, result);
    }

    #endregion

    #region TrimStart

    [TestMethod]
    public void TrimStart_WithMatchingStart_RemovesStartString()
    {
        var value = "BoufBouftou";

        var result = value.TrimStart("Bouf");

        Assert.AreEqual("tou", result);
    }

    [TestMethod]
    public void TrimStart_WithNoMatch_ReturnsOriginal()
    {
        var value = "Bouftou";

        var result = value.TrimStart("tou");

        Assert.AreEqual(value, result);
    }

    #endregion

    #region TrimEnd

    [TestMethod]
    public void TrimEnd_WithMatchingEnd_RemovesEndString()
    {
        var value = "Bouftoutou";

        var result = value.TrimEnd("tou");

        Assert.AreEqual("Bouf", result);
    }

    [TestMethod]
    public void TrimEnd_WithNoMatch_ReturnsOriginal()
    {
        var value = "Bouftou";

        var result = value.TrimEnd("Bou");

        Assert.AreEqual(value, result);
    }

    #endregion

    #region ToNumberOrZeroFromHex

    [TestMethod]
    public void ToNumberOrZeroFromHex_WithValidHex_ReturnsNumber()
    {
        var value = "1A";

        var result = value.ToNumberOrZeroFromHex<int>();

        Assert.AreEqual(26, result);
    }

    [TestMethod]
    public void ToNumberOrZeroFromHex_WithValidNegativeHex_ReturnsNumber()
    {
        var value = "-1A";

        var result = value.ToNumberOrZeroFromHex<int>();

        Assert.AreEqual(-26, result);
    }

    [TestMethod]
    public void ToNumberOrZeroFromHex_WithInvalidHex_ReturnsDefault()
    {
        var value = "XYZ";

        var result = value.ToNumberOrZeroFromHex<int>();

        Assert.AreEqual(default, result);
    }

    #endregion

    #region NormalizeToAscii

    [TestMethod]
    public void NormalizeToAscii_WithAccentedCharacters_ReturnsAsciiEquivalent()
    {
        var value = "ÀÁÂÃÄÅ";

        var result = value.NormalizeToAscii();

        Assert.AreEqual("AAAAAA", result);
    }

    [TestMethod]
    public void NormalizeToAscii_WithSpecialCharacters_MapsToDefinedCharacters()
    {
        var value = "ȹ";

        var result = value.NormalizeToAscii();

        Assert.AreEqual("qp", result);
    }

    [TestMethod]
    public void NormalizeToAscii_WithMixedContent_NormalizesCorrectly()
    {
        var value = "Féca ȹ";

        var result = value.NormalizeToAscii();

        Assert.AreEqual("Feca qp", result);
    }

    [TestMethod]
    public void NormalizeToAscii_WithNonTargetedCharacters_LeavesThemUnchanged()
    {
        var value = "Feca FTW 69420";

        var result = value.NormalizeToAscii();

        Assert.AreEqual(value, result);
    }

    #endregion
}
