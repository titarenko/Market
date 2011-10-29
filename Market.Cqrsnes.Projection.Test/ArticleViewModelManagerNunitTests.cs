using NUnit.Framework;

namespace Market.Cqrsnes.Projection.Test
{
    [TestFixture]
    public class ArticleViewModelManagerNunitTests
    {
        private ArticleViewModelManagerSpecifications specifications;

        [SetUp]
        public void SetUp()
        {
            specifications = new ArticleViewModelManagerSpecifications();
        }

        [Test]
        public void ArticleCreated()
        {
            specifications.ArticleCreated().AssertResult();
        }

        [Test]
        public void ArticleDelivered()
        {
            specifications.ArticleDelivered().AssertResult();
        }

        [Test]
        public void ArticleBought()
        {
            specifications.ArticleBought().AssertResult();
        }

        [Test]
        public void DuplicateArticleCreated()
        {
            specifications.DuplicateArticleCreated().AssertResult();
        }

        [Test]
        public void BoughtMoreThanExist()
        {
            specifications.BoughtMoreThanExist().AssertResult();
        }
    }
}