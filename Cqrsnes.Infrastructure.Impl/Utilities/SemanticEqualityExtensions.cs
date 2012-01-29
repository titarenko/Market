using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cqrsnes.Infrastructure.Impl.Utilities
{
    public static class SemanticEqualityExtensions
    {
        /// <summary>
        /// Compares two objects for equality.
        /// </summary>
        /// <param name="lhs">First object.</param>
        /// <param name="rhs">Second object.</param>
        /// <returns>
        /// True if objects have the same type and their public properties 
        /// have same values and false otherwise.
        /// </returns>
        public static bool SemanticallyEquals(this object lhs, object rhs)
        {
            return lhs == null && rhs == null ||
                   lhs != null && rhs != null &&
                   rhs.GetType() == lhs.GetType() &&
                   lhs.GetType()
                       .GetProperties()
                       .All(property =>
                                {
                                    var lhsValue = property.GetValue(lhs, null);
                                    var rhsValue = property.GetValue(rhs, null);
                                    return lhsValue == null && rhsValue == null ||
                                           lhsValue != null && rhsValue != null &&
                                           lhsValue.Equals(rhsValue);
                                });
        }

        /// <summary>
        /// Compares two sequences using same rules 
        /// as described for <see cref="SemanticallyEquals"/> for objects.
        /// </summary>
        /// <param name="lhs">First sequence.</param>
        /// <param name="rhs">Second sequence.</param>
        /// <returns>True if sequences are equal, false otherwise.</returns>
        public static bool SemanticallyEquals(this IEnumerable lhs, IEnumerable rhs)
        {
            return lhs == null && rhs == null ||
                   lhs != null && rhs != null &&
                   lhs.Cast<object>().SequenceEqual<object>(
                       rhs.Cast<object>(), new EqualityComparer());
        }

        private class EqualityComparer : IEqualityComparer<object>
        {
            public bool Equals(object x, object y)
            {
                return x.SemanticallyEquals(y);
            }

            public int GetHashCode(object obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
