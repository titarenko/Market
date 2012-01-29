using System.Collections.Generic;

namespace Cqrsnes.CodeGeneration
{
    public interface IDslParser
    {
        Entity Parse(string line);

        IEnumerable<Entity> Parse(IEnumerable<string> lines);
    }
}
