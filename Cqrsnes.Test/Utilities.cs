using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Cqrsnes.Test
{
    /// <summary>
    /// Contains utility methods.
    /// </summary>
    public static class Utilities
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
        public static bool ObjectsAreEqual(object lhs, object rhs)
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
        /// as described for <see cref="ObjectsAreEqual"/>.
        /// </summary>
        /// <param name="lhs">First sequence.</param>
        /// <param name="rhs">Second sequence.</param>
        /// <returns>True if sequences are equal, false otherwise.</returns>
        public static bool SequenceEqual(IEnumerable lhs, IEnumerable rhs)
        {
            return lhs == null && rhs == null || lhs != null && rhs != null &&
                lhs.Cast<object>().SequenceEqual(rhs.Cast<object>(), new EqualityComparer());
        }

        /// <summary>
        /// Converts Guid to human readable string.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string Prettify(Guid guid)
        {
            var text = guid.ToString();
            return text.Substring(0, 2) + "..." + text.Substring(text.Length - 2);
        }

        /// <summary>
        /// Converts symbol name to human readable string (e.g. 
        /// CanPrettify -> can prettify, can_prettify -> can prettify).
        /// </summary>
        /// <param name="name">Symbol name.</param>
        /// <returns>Human readable symbol name.</returns>
        public static string Prettify(string name)
        {
            if (name.ToLower().StartsWith("get"))
            {
                name = name.Substring(3);
            }

            return Regex
                .Replace(name, "([A-Z])", " $1", RegexOptions.Compiled)
                .TrimStart()
                .Replace('_', ' ')
                .ToLower();
        }

        /// <summary>
        /// Describes object (prints its type name and 
        /// public properties as names/values) in human readable way.
        /// </summary>
        /// <param name="instance">Object to describe.</param>
        /// <returns>Description (as human readable text).</returns>
        public static string Describe(object instance)
        {
            if (instance == null)
            {
                return "null";
            }

            var type = instance.GetType();
            if (type.Namespace.StartsWith("System"))
            {
                var formattable = instance as IFormattable;
                return formattable != null
                           ? formattable.ToString(null, CultureInfo.InvariantCulture)
                           : instance.ToString();
            }

            var name = Prettify(type.Name);
            var properties = type.GetProperties();

            var builder = new StringBuilder(name);
            builder.Append(" (");

            var first = true;
            foreach (var property in properties)
            {
                var value = property.GetValue(instance, null);

                if (!first)
                {
                    builder.Append(", ");
                }
                else
                {
                    first = false;
                }

                builder.AppendFormat(
                    "{0}: \"{1}\"",
                    Prettify(property.Name),
                    value == null
                        ? "null"
                        : (value.GetType() == typeof (Guid)
                               ? Prettify((Guid) value)
                               : Describe(value)));
            }

            builder.Append(")");

            return builder.ToString();
        }

        public static string DescribeAction<T>(Expression<Action<T>> expression)
        {
            var body = expression.Body as MethodCallExpression;

            if (body == null)
            {
                throw new ApplicationException("Only method calls are supported as actions.");
            }

            Func<MemberBinding, string> memberSelector = x =>
            {
                var memberAssignment = x as MemberAssignment;
                if (memberAssignment == null)
                {
                    throw new ApplicationException("Only member assignments are supported.");
                }

                var value = Expression.Lambda(memberAssignment.Expression).Compile().DynamicInvoke();

                return string.Format(
                    "{0}: \"{1}\"", 
                    Utilities.Prettify(x.Member.Name), 
                    value);
            };

            Func<Expression, string> argumentSelector = y =>
            {
                var memberInitExpression = y as MemberInitExpression;
                if (memberInitExpression == null)
                {
                    throw new ApplicationException(
                        "Only member initialization expressions are supported as arguments for method call.");
                }

                return string.Format(
                    "{0} ({1})", 
                    Utilities.Prettify(memberInitExpression.Type.Name), 
                    string.Join(", ", memberInitExpression.Bindings.Select(memberSelector)));
            };

            return string.Format(
                "{0} with {1}", 
                Utilities.Prettify(body.Method.Name), 
                string.Join(" and ", body.Arguments.Select(argumentSelector)));
        }

        private class EqualityComparer : IEqualityComparer<object>
        {
            public bool Equals(object x, object y)
            {
                return ObjectsAreEqual(x, y);
            }

            public int GetHashCode(object obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}