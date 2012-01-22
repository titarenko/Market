using System;

namespace Market.Cqrsnes.Domain.Utility
{
    public static class ValidationExtensions
    {
        public static void ShouldBePositive(this int value, string name)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(name, "Should be positive.");
            }
        }

        public static void ShouldBePositive(this double value, string name)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(name, "Should be positive.");
            }
        }

        public static void ShouldNotBeEmpty(this string @string, string name)
        {
            if (string.IsNullOrWhiteSpace(@string))
            {
                throw new ArgumentException("Should not be empty.", name);
            }
        }

        public static void ShouldNotBeEmpty(this Guid id, string name)
        {
            if (id == default(Guid))
            {
                throw new ArgumentException("Should not be empty.", name);
            }
        }
    }
}