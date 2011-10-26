using System.Collections.Generic;

namespace Market.Cqrsnes.Test
{
    public interface ISpecificationHolder
    {
        IEnumerable<ExecutionResult> ExecuteAll();
    }
}