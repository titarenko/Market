using System.Collections.Generic;

namespace Cqrsnes.Infrastructure.CodeGeneration
{
    public class Entity
    {
        public EntityType Type { get; set; }

        public string Name { get; set; }

        public IEnumerable<Attribute> Attributes { get; set; }
    }
}
