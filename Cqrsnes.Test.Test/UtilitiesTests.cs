using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;

namespace Cqrsnes.Test.Test
{
    [TestFixture]
    public class UtilitiesTests
    {
        [Test]
        [TestCase("CanPrettifyThis", "can prettify this")]
        [TestCase("Can_Prettify_This", "can prettify this")]
        [TestCase("CanPrettify_This", "can prettify this")]
        [TestCase("canPrettify_this", "can prettify this")]
        public void CanPrettify(string given, string expected)
        {
            Assert.AreEqual(expected, Utilities.Prettify(expected));
        }

        [Test, TestCase("992F1BB0-2750-4FC7-B972-CC0AF451957C", "99...7c")]
        public void CanPrettifyGuid(string guid, string prettyGuid)
        {
            Assert.AreEqual(prettyGuid, Utilities.Prettify(Guid.Parse(guid)));
        }

        [Test, TestCaseSource("ActionsToDescribe")]
        public void CanDescribeAction(object expression, string expectedDescription)
        {
            Assert.AreEqual(expectedDescription, Utilities.DescribeAction(expression as Expression<Action<TypeC>>));
        }

        public static IEnumerable<TestCaseData> ActionsToDescribe
        {
            get
            {
                yield return new TestCaseData(GetExpression<TypeC>(x => x.SetA(new TypeA
                {
                    NumberProperty = 1,
                    StringProperty = "Abc"
                })), "set a with type a (number property: \"1\", string property: \"Abc\")");

                yield return new TestCaseData(GetExpression<TypeC>(
                    x => x.SetAB(
                        new TypeA
                        {
                            NumberProperty = 1,
                            StringProperty = "Abc"
                        },
                        new TypeB
                        {
                            NumberProperty = 2,
                            StringProperty = "Def"
                        })), "set a b with type a (number property: \"1\", string property: \"Abc\") and type b (number property: \"2\", string property: \"Def\")");
            }
        }
        
        [Test, TestCaseSource("ObjectsToDescribe")]
        public void CanDescribe(object instance, string expectedDescription)
        {
            Assert.AreEqual(expectedDescription, Utilities.Describe(instance));
        }

        public static IEnumerable<TestCaseData> ObjectsToDescribe
        {
            get
            {
                yield return new TestCaseData(null, "null");

                yield return new TestCaseData(1, "1");
                yield return new TestCaseData(1.23, "1.23");
                yield return new TestCaseData("Some string.", "Some string.");

                yield return new TestCaseData(
                    new TypeA
                        {
                            NumberProperty = 1,
                            StringProperty = "String value."
                        },
                    "type a (number property: \"1\", string property: \"String value.\")");

                yield return new TestCaseData(
                    new TypeC
                    {
                        AValue = new TypeA
                            {
                                NumberProperty = 5
                            },
                        BValue = new TypeB
                            {
                                StringProperty = "String value."
                            }
                    },
                    "type c (a value: \"type a (number property: \"5\", string property: \"null\")\", b value: \"type b (number property: \"0\", string property: \"String value.\")\")");
            }
        }

        private static Expression<Action<T>> GetExpression<T>(Expression<Action<T>> expression)
        {
            return expression;
        }
            
        [Test, TestCaseSource("ObjectsToCompareForEquality")]
        public void CanCompareObjectsForEquality(object lhs, object rhs, bool expected)
        {
            Assert.AreEqual(expected, Utilities.ObjectsAreEqual(lhs, rhs));
        }

        public static IEnumerable<TestCaseData> ObjectsToCompareForEquality
        {
            get
            {
                yield return new TestCaseData(new object(), new object(), true);
                yield return new TestCaseData(null, null, true);

                yield return new TestCaseData(new object(), null, false);
                yield return new TestCaseData(null, new object(), false);

                yield return new TestCaseData(new TypeA(), new TypeA(), true);
                yield return new TestCaseData(new TypeA(), new TypeB(), false);

                yield return new TestCaseData(
                    new TypeA
                        {
                            NumberProperty = 1,
                            StringProperty = "Test"
                        },
                    new TypeB
                        {
                            NumberProperty = 1,
                            StringProperty = "Test"
                        },
                    false);

                yield return new TestCaseData(
                    new TypeA
                    {
                        NumberProperty = 1,
                        StringProperty = "Test"
                    },
                    new TypeA
                    {
                        NumberProperty = 1,
                        StringProperty = "Test"
                    },
                    true);

                yield return new TestCaseData(
                    new TypeA
                    {
                        NumberProperty = 1,
                        StringProperty = "Test"
                    },
                    new TypeA
                    {
                        NumberProperty = 1,
                        StringProperty = null
                    },
                    false);
            }
        }

        class TypeA
        {
            public int NumberProperty { get; set; }

            public string StringProperty { get; set; }
        }

        class TypeB
        {
            public int NumberProperty { get; set; }

            public string StringProperty { get; set; }
        }

        class TypeC
        {
            public TypeA AValue { get; set; }

            public TypeB BValue { get; set; }

            public void SetA(TypeA a)
            {
                AValue = a;
            }

            public void SetAB(TypeA a, TypeB b)
            {
                AValue = a;
                BValue = b;
            }
        }

        [Test, TestCaseSource("SequencesToCompareForEquality")]
        public void CanCompareSequencesForEquality(
            IEnumerable lhs, IEnumerable rhs, bool expected)
        {
            Assert.AreEqual(expected, Utilities.SequenceEqual(lhs, rhs));
        }

        public static IEnumerable<TestCaseData> SequencesToCompareForEquality
        {
            get
            {
                yield return new TestCaseData(null, null, true);
                yield return new TestCaseData(null, new object[0], false);
                yield return new TestCaseData(new object[0], null, false);
                yield return new TestCaseData(new object[0], new object[0], true);

                yield return new TestCaseData(new object[] {null, null}, new object[] {null, null}, true);
                yield return new TestCaseData(new object[] {null}, new object[] {null, null}, false);

                yield return new TestCaseData(
                    new object[]
                        {
                            new TypeA
                                {
                                    NumberProperty = 1
                                },
                            new TypeB
                                {
                                    NumberProperty = 2
                                }
                        },
                    new object[]
                        {
                            new TypeB
                                {
                                    NumberProperty = 2
                                },
                            new TypeA
                                {
                                    NumberProperty = 1
                                }
                        },
                    false)
                    .SetName("Same types, different order, simple properties - not equal.");

                yield return new TestCaseData(
                    new object[]
                        {
                            new TypeA
                                {
                                    NumberProperty = 1,
                                    StringProperty = "Abc"
                                },
                            new TypeA
                                {
                                    NumberProperty = 3,
                                    StringProperty = "Bcd"
                                },
                            new TypeB
                                {
                                    NumberProperty = 2,
                                    StringProperty = "Def"
                                }
                        },
                    new object[]
                        {
                            new TypeA
                                {
                                    NumberProperty = 3,
                                    StringProperty = "Bcd"
                                },
                            new TypeB
                                {
                                    NumberProperty = 2,
                                    StringProperty = "Def"
                                },
                            new TypeA
                                {
                                    NumberProperty = 1,
                                    StringProperty = "Abc"
                                }
                        },
                    false)
                    .SetName("Same objects, different order - not equal.");

                yield return new TestCaseData(
                    new object[]
                        {
                            new TypeA
                                {
                                    NumberProperty = 1,
                                    StringProperty = "Abc"
                                },
                            new TypeA
                                {
                                    NumberProperty = 3,
                                    StringProperty = "Bcd"
                                }
                        },
                    new object[]
                        {
                            new TypeA
                                {
                                    NumberProperty = 3,
                                    StringProperty = "Bcd"
                                },
                            new TypeA
                                {
                                    NumberProperty = 1,
                                    StringProperty = null
                                }
                        },
                    false)
                    .SetName("Same types, different values - not equal.");
            }
        }
    }
}
