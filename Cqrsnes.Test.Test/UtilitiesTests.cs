using System;
using System.Collections;
using System.Collections.Generic;
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
                            Number = 1,
                            String = "Test"
                        },
                    new TypeB
                        {
                            Number = 1,
                            String = "Test"
                        },
                    false);

                yield return new TestCaseData(
                    new TypeA
                    {
                        Number = 1,
                        String = "Test"
                    },
                    new TypeA
                    {
                        Number = 1,
                        String = "Test"
                    },
                    true);

                yield return new TestCaseData(
                    new TypeA
                    {
                        Number = 1,
                        String = "Test"
                    },
                    new TypeA
                    {
                        Number = 1,
                        String = null
                    },
                    false);
            }
        }

        class TypeA
        {
            public int Number { get; set; }

            public string String { get; set; }
        }

        class TypeB
        {
            public int Number { get; set; }

            public string String { get; set; }
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
                                    Number = 1
                                },
                            new TypeB
                                {
                                    Number = 2
                                }
                        },
                    new object[]
                        {
                            new TypeB
                                {
                                    Number = 2
                                },
                            new TypeA
                                {
                                    Number = 1
                                }
                        },
                    false)
                    .SetName("Same types, different order, simple properties - not equal.");

                yield return new TestCaseData(
                    new object[]
                        {
                            new TypeA
                                {
                                    Number = 1,
                                    String = "Abc"
                                },
                            new TypeA
                                {
                                    Number = 3,
                                    String = "Bcd"
                                },
                            new TypeB
                                {
                                    Number = 2,
                                    String = "Def"
                                }
                        },
                    new object[]
                        {
                            new TypeA
                                {
                                    Number = 3,
                                    String = "Bcd"
                                },
                            new TypeB
                                {
                                    Number = 2,
                                    String = "Def"
                                },
                            new TypeA
                                {
                                    Number = 1,
                                    String = "Abc"
                                }
                        },
                    false)
                    .SetName("Same objects, different order - not equal.");

                yield return new TestCaseData(
                    new object[]
                        {
                            new TypeA
                                {
                                    Number = 1,
                                    String = "Abc"
                                },
                            new TypeA
                                {
                                    Number = 3,
                                    String = "Bcd"
                                }
                        },
                    new object[]
                        {
                            new TypeA
                                {
                                    Number = 3,
                                    String = "Bcd"
                                },
                            new TypeA
                                {
                                    Number = 1,
                                    String = null
                                }
                        },
                    false)
                    .SetName("Same types, different values - not equal.");
            }
        }
    }
}
