using Cqrsnes.Test;
using NUnit.Framework;

namespace Market.Cqrsnes.Projection.Test
{
    public static class Extensions
    {
        public static void AssertResult(this ExecutionResult result)
        {
            Assert.IsTrue(result.IsPassed, result.Details);
        }
    }
}