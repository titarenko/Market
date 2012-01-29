using System;
using System.Collections.Generic;
using System.Linq;
using Cqrsnes.Infrastructure.CodeGeneration;
using Attribute = Cqrsnes.Infrastructure.CodeGeneration.Attribute;

namespace Cqrsnes.Infrastructure.Impl.CodeGeneration
{
    /// <summary>
    /// Default DSL (domain-specific language) parser.
    /// </summary>
    public class DslParser : IDslParser
    {
        public Entity Parse(string line)
        {
            var nameAndAttributes = line.Split(':', '!').Select(x => x.Trim()).ToArray();
            if (nameAndAttributes.Length != 2)
            {
                throw new ApplicationException(
                    "Can't find \"<name>{:|!}<attributes>\" in the given line.");
            }

            if (nameAndAttributes[0].Length == 0)
            {
                throw new ApplicationException("Name must not be empty.");
            }

            return new Entity
                {
                    Name = nameAndAttributes[0],
                    Type = line.IndexOf(':') > 0 ? EntityType.Command : EntityType.Event,
                    Attributes = ParseAttributes(nameAndAttributes[1])
                };
        }

        private IEnumerable<Attribute> ParseAttributes(string attributes)
        {
            return attributes
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x =>
                            {
                                var pair = x
                                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(y => y.Trim())
                                    .ToArray();

                                if (pair.Length < 1 || pair.Length > 2)
                                {
                                    throw new ApplicationException(
                                        "Can't find \"<name>\" or \"<type> <name>\" expression in the given line.");
                                }

                                var hasType = pair.Length == 2;
                                return new Attribute
                                    {
                                        Name = hasType ? pair[1] : pair[0],
                                        Type = hasType ? GetType(pair[0]) : GuessType(pair[0])
                                    };
                            })
                .ToArray();
        }

        private AttributeType GetType(string name)
        {
            switch (name.ToLower())
            {
                case "guid":
                    return AttributeType.Guid;

                case "int":
                    return AttributeType.Int;

                case "double":
                    return AttributeType.Double;

                case "string":
                    return AttributeType.String;

                default:
                    throw new ArgumentException("Unknown attribute type name.", "name");
            }
        }

        private AttributeType GuessType(string name)
        {
            name = name.ToLower();

            if (name.Contains("id"))
            {
                return AttributeType.Guid;
            }

            if (name.Contains("count") || 
                name.Contains("size") || 
                name.Contains("length"))
            {
                return AttributeType.Int;
            }

            return AttributeType.String;
        }
    }
}
