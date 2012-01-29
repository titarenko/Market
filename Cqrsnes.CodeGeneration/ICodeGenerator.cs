using System.Collections.Generic;

namespace Cqrsnes.CodeGeneration
{
    public interface ICodeGenerator
    {
        string Indent { get; set; }

        string Generate(Entity entity);

        string Generate(IEnumerable<Entity> entities);
    }
}
