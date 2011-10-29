using System.Collections.Generic;

namespace Cqrsnes.Test
{
    public interface ISpecificationHolder
    {
        IEnumerable<ExecutionResult> ExecuteAll();
    }
}