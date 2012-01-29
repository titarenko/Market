using System.Collections;
using System.Collections.Generic;
using Cqrsnes.Infrastructure.Impl.Utilities;
using NUnit.Framework;

namespace Cqrsnes.Infrastructure.Impl.Test
{
    public class SemanticEqualityExtensionsTests
    {
        [Test, TestCaseSource("ObjectsToCompareForEquality")]
        public void CanCompareObjectsForEquality(object lhs, object rhs, bool expected)
        {
            Assert.AreEqual(expected, lhs.SemanticallyEquals(rhs));
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
            Assert.AreEqual(expected, lhs.SemanticallyEquals(rhs));
        }

        public static IEnumerable<TestCaseData> SequencesToCompareForEquality
        {
            get
            {
                yield return new TestCaseData(null, null, true);
                yield return new TestCaseData(null, new object[0], false);
                yield return new TestCaseData(new object[0], null, false);
                yield return new TestCaseData(new object[0], new object[0], true);

                yield return new TestCaseData(new object[] { null, null }, new object[] { null, null }, true);
                yield return new TestCaseData(new object[] { null }, new object[] { null, null }, false);

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