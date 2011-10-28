using NUnit.Framework;

namespace Market.Cqrsnes.Web.Test
{
    public static class Extensions
    {
        public static void AssertResult(this ExecutionResult result)
        {
            Assert.IsTrue(result.IsPassed, result.Description);
        }
    }
}