namespace Cqrsnes.Test
{
    /// <summary>
    /// Specification definition.
    /// </summary>
    public interface ISpecification
    {
        /// <summary>
        /// Gets name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Executes specification.
        /// </summary>
        /// <returns>Execution result.</returns>
        ExecutionResult Run();
    }
}
