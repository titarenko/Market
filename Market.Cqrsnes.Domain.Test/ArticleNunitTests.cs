using NUnit.Framework;

namespace Market.Cqrsnes.Domain.Test
{
    [TestFixture]
    public class ArticleNunitTests
    {
        private ArticleSpecifications specifications;

        [SetUp]
        public void SetUp()
        {
            specifications = new ArticleSpecifications();
        }

        [Test]
        public void Create()
        {
            specifications.Create().AssertResult();
        }

        [Test]
        public void Deliver()
        {
            specifications.Deliver().AssertResult();
        }

        [Test]
        public void BuyLessThanExists()
        {
            specifications.BuyLessThanExist().AssertResult();
        }

        [Test]
        public void BuyMoreThanExists()
        {
            specifications.BuyMoreThanExist().AssertResult();
        }
    }
}