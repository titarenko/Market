using NUnit.Framework;

namespace Cqrsnes.Test.Test
{
    [TestFixture]
    public class UtilitiesTests
    {
        [Test]
        [TestCase("CanPrettifyThis", "can prettify this")]
        [TestCase("Can_Prettify_This", "can prettify this")]
        [TestCase("CanPrettify_This", "can prettify this")]
        [TestCase("canPrettify_this", "can prettify this")]
        public void CanPrettify(string given, string expected)
        {
            Assert.AreEqual(expected, Utilities.Prettify(expected));
        }
    }
}
