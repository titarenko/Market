using NUnit.Framework;

namespace Market.Cqrsnes.Test
{
    public static class ExtensionMethods
    {
        public static void AssertResult(this ExecutionResult result)
        {
            Assert.IsTrue(result.IsPassed, result.Details);
        }
    }
}