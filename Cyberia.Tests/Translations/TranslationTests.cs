using Cyberia.Translations;

namespace Cyberia.Tests.Translations;

[TestClass]
public sealed class TranslationTests
{
    #region Format

    [TestMethod]
    public void Format_WithOneParameter_ReturnFormatedString()
    {
        var result = Translation.Format("Hello, #1!", 1);

        Assert.AreEqual("Hello, 1!", result);
    }

    [TestMethod]
    public void Format_WithTwoParameters_ReturnFormatedString()
    {
        var result = Translation.Format("Hello, #1 #2!", "John", 2);

        Assert.AreEqual("Hello, John 2!", result);
    }

    [TestMethod]
    public void Format_WithThreeParameters_ReturnFormatedString()
    {
        var result = Translation.Format("Hello, #1 #2 #3!", "John", "Doe", 3);

        Assert.AreEqual("Hello, John Doe 3!", result);
    }

    [TestMethod]
    public void Format_WithMultipleParameters_ReturnFormatedString()
    {
        var result = Translation.Format("Hello, #1 #2 #3 #4!", "Mr.", "John", "Doe", 4);

        Assert.AreEqual("Hello, Mr. John Doe 4!", result);
    }

    [TestMethod]
    public void Format_WithMultipleStringParameters_ReturnFormatedString()
    {
        var result = Translation.Format("Hello, #1 #2 #3 #4!", "Mr.", "John", "Doe", "Jr.");

        Assert.AreEqual("Hello, Mr. John Doe Jr.!", result);
    }

    [TestMethod]
    public void Format_WithNoParameters_ReturnFormatedString()
    {
        var result = Translation.Format("Hello, World!", ReadOnlySpan<string>.Empty);

        Assert.AreEqual("Hello, World!", result);
    }

    [TestMethod]
    public void Format_WithConditionalParameters_ReturnFormatedString()
    {
        var result = Translation.Format("Hello, #1{~1~2 and }#2!", "John", "Bob");
        var result2 = Translation.Format("Hello, #1{~1~2 and }#2!", "John", string.Empty);
        var result3 = Translation.Format("Hello, #1{~1~2 and }#2!", string.Empty, "Bob");

        Assert.AreEqual("Hello, John and Bob!", result);
        Assert.AreEqual("Hello, John!", result2);
        Assert.AreEqual("Hello, Bob!", result3);
    }

    #endregion
}
