using System;
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
    }
}
