using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cqrsnes.CodeGeneration
{
    public class CodeGenerator : ICodeGenerator
    {
        public string Indent { get; set; }

        public string Generate(Entity entity)
        {
            var s = new StringBuilder();

            s.Append(Indent);
            s.AppendFormat(
                "public class {0} : {1}\n", 
                entity.Name, 
                entity.Type == EntityType.Command ? "Command" : "Event");

            s.Append(Indent);
            s.AppendLine("{");

            foreach (var attribute in entity.Attributes)
            {
                s.Append(Indent);
                s.AppendFormat(
                    "\tpublic {0} {1} {{ get; set; }}\n",
                    GetType(attribute),
                    attribute.Name);
            }

            s.Append(Indent);
            s.Append("}");

            return s.ToString();
        }

        private static string GetType(Attribute attribute)
        {
            switch (attribute.Type)
            {
                case AttributeType.Guid:
                    return "Guid";

                case AttributeType.Int:
                    return "int";

                case AttributeType.Double:
                    return "double";

                case AttributeType.String:
                    return "string";

                default:
                    throw new ApplicationException("Unknown attribute type.");
            }
        }

        public string Generate(IEnumerable<Entity> entities)
        {
            return string.Join(
                Environment.NewLine + Environment.NewLine, 
                entities.Select(Generate));
        }
    }
}
