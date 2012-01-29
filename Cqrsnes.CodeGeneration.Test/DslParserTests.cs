using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Cqrsnes.CodeGeneration.Test
{
    [TestFixture]
    public class DslParserTests
    {
        private IDslParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new DslParser();
        }

        [Test, TestCaseSource("LinesToParse")]
        public void CanParse(string line, Entity expected)
        {
            var actual = parser.Parse(line);

            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Type, actual.Type);

            var expectedAttributes = expected.Attributes.ToArray();
            var actualAttributes = actual.Attributes.ToArray();

            Assert.AreEqual(expectedAttributes.Length, actualAttributes.Length);

            for (int i = 0, count = expectedAttributes.Length; i < count; i++)
            {
                Assert.AreEqual(expectedAttributes[i].Name, actualAttributes[i].Name);
                Assert.AreEqual(expectedAttributes[i].Type, actualAttributes[i].Type);
            }
        }

        public static IEnumerable<TestCaseData> LinesToParse
        {
            get
            {
                yield return new TestCaseData(
                    "CreateUser:",
                    new Entity
                        {
                            Name = "CreateUser",
                            Type = EntityType.Command,
                            Attributes = new Attribute[0]
                        });

                yield return new TestCaseData(
                    "UserCreated! Id, Name, int Rank, double Balance, CarsCount, FootSize, NoseLength, double HairLength",
                    new Entity
                        {
                            Name = "UserCreated",
                            Type = EntityType.Event,
                            Attributes = new[]
                                {
                                    new Attribute
                                        {
                                            Name = "Id",
                                            Type = AttributeType.Guid
                                        },
                                    new Attribute
                                        {
                                            Name = "Name",
                                            Type = AttributeType.String
                                        },
                                    new Attribute
                                        {
                                            Name = "Rank",
                                            Type = AttributeType.Int
                                        },
                                    new Attribute
                                        {
                                            Name = "Balance",
                                            Type = AttributeType.Double
                                        },
                                    new Attribute
                                        {
                                            Name = "CarsCount",
                                            Type = AttributeType.Int
                                        },
                                    new Attribute
                                        {
                                            Name = "FootSize",
                                            Type = AttributeType.Int
                                        },
                                    new Attribute
                                        {
                                            Name = "NoseLength",
                                            Type = AttributeType.Int
                                        },
                                    new Attribute
                                        {
                                            Name = "HairLength",
                                            Type = AttributeType.Double
                                        }
                                }
                        });
            }
        }
    }
}
