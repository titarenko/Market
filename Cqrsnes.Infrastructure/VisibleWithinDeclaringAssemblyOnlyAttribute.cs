using System;

namespace Cqrsnes.Infrastructure
{
    /// <summary>
    /// Messages with this attribute will be delivered
    /// to handlers within declaring assembly only.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class VisibleWithinDeclaringAssemblyOnlyAttribute : Attribute
    {
    }
}
