using NUnit.Framework;

namespace Market.Cqrsnes.Web.Test
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
    }
}