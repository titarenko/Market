using System;
using System.Collections.Generic;
using System.Linq;
using Cqrsnes.Infrastructure;
using Cqrsnes.Test;
using Market.Cqrsnes.Domain;

namespace Market.Cqrsnes.Projection.Test
{
    public class ArticleViewModelManagerSpecifications : ISpecificationHolder
    {
        private readonly Guid id = Guid.NewGuid();

        private readonly string name = "New Article";

        public ExecutionResult ArticleCreated()
        {
            return new ProjectionSpecification<ArticleViewModelManager>()
                .When(new ArticleCreated(id, name))
                .Expect(x => x.GetArticleListViewModel().Articles.Count == 1)
                .Expect(x => x.GetArticleListViewModel().Articles.First().Id == id)
                .Expect(x => x.GetArticleListViewModel().Articles.First().Name == name)
                .Run();
        }

        public ExecutionResult ArticleDelivered()
        {
            return new ProjectionSpecification<ArticleViewModelManager>()
                .Given(new[] {new ArticleCreated(id, name)})
                .When(new ArticleDelivered(id, 15))
                .Expect(x => x.GetArticleListViewModel().Articles.First().Id == id)
                .Expect(x => x.GetArticleListViewModel().Articles.First().Count == 15)
                .Run();
        }

        public ExecutionResult ArticleBought()
        {
            return new ProjectionSpecification<ArticleViewModelManager>()
                .Given(new Event[]
                           {
                               new ArticleCreated(id, name),
                               new ArticleDelivered(id, 20)
                           })
                .When(new ArticleBought(id, 15))
                .Expect(x => x.GetArticleListViewModel().Articles.First().Id == id)
                .Expect(x => x.GetArticleListViewModel().Articles.First().Count == 5)
                .Run();
        }

        public ExecutionResult DuplicateArticleCreated()
        {
            return new ProjectionSpecification<ArticleViewModelManager>()
                .Given(new[] {new ArticleCreated(id, name)})
                .When(new ArticleCreated(id, name))
                .ExpectException()
                .Run();
        }

        public ExecutionResult BoughtMoreThanExist()
        {
            return new ProjectionSpecification<ArticleViewModelManager>()
                .Given(new Event[]
                           {
                               new ArticleCreated(id, name),
                               new ArticleDelivered(id, 10)
                           })
                .When(new ArticleBought(id, 100))
                .ExpectException()
                .Run();
        }

        public IEnumerable<ExecutionResult> ExecuteAll()
        {
            return new[]
                       {
                           ArticleCreated(),
                           ArticleDelivered(),
                           ArticleBought(),
                           DuplicateArticleCreated(),
                           BoughtMoreThanExist()
                       };
        }
    }
}