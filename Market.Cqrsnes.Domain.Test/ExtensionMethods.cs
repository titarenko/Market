using Cqrsnes.Test;
using NUnit.Framework;

namespace Market.Cqrsnes.Domain.Test
{
    public static class ExtensionMethods
    {
        public static void AssertResult(this ExecutionResult result)
        {
            Assert.IsTrue(result.IsPassed, result.Details);
        }
    }
}